#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Process is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_PROCESS
#nullable enable
#pragma warning disable CS0436

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_Process
{
    private static int ToTimeoutMilliseconds(TimeSpan? timeout)
    {
        if (timeout is not { } value)
            return Timeout.Infinite;

        var totalMilliseconds = (long)value.TotalMilliseconds;

        ArgumentOutOfRangeException.ThrowIfLessThan(totalMilliseconds, -1L, nameof(timeout));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(
            totalMilliseconds,
            int.MaxValue,
            nameof(timeout)
        );

        return (int)totalMilliseconds;
    }

    private static int GetRemainingMilliseconds(Stopwatch stopwatch, int timeoutMilliseconds) =>
        timeoutMilliseconds == Timeout.Infinite
            ? Timeout.Infinite
            : Math.Max(0, timeoutMilliseconds - (int)stopwatch.ElapsedMilliseconds);

    private static ProcessStartInfo CreateStartInfo(
        string fileName,
        IEnumerable<string>? arguments,
        bool redirectStreams = false
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName);

        var startInfo = new ProcessStartInfo(fileName)
        {
            // Defaults to true on .NET Framework
            UseShellExecute = false,
        };

        if (arguments is not null)
        {
            foreach (var argument in arguments)
                startInfo.ArgumentList.Add(argument);
        }

        startInfo.RedirectStandardOutput = redirectStreams;
        startInfo.RedirectStandardError = redirectStreams;

        return startInfo;
    }

    // Runs the process with its output redirected, drained, and discarded (see Run for why). Unlike
    // RunAndCaptureText, the timeout cancels the process instead of failing, same as without silent.
    private static ProcessExitStatus RunSilent(
        string fileName,
        IEnumerable<string>? arguments,
        TimeSpan? timeout
    )
    {
        var startInfo = CreateStartInfo(fileName, arguments, true);

        // Validate before starting the process
        var timeoutMilliseconds = ToTimeoutMilliseconds(timeout);

        var stopwatch = Stopwatch.StartNew();

        using var process =
            Process.Start(startInfo)
            ?? throw new InvalidOperationException("The process could not be started.");

        try
        {
            // The streams must be drained, otherwise the process may block on a full pipe buffer
            foreach (var _ in process.ReadBytes(timeoutMilliseconds)) { }
        }
        // No time is left, so the process is killed below unless it has already exited
        catch (TimeoutException) { }
        catch
        {
            process.TryKill();
            throw;
        }

        return process.WaitForExitStatusOrKill(
            TimeSpan.FromMilliseconds(GetRemainingMilliseconds(stopwatch, timeoutMilliseconds))
        );
    }

    // Task infrastructure is required for async method support
#if FEATURE_TASK
    // Async counterpart of RunSilent(...)
    private static async Task<ProcessExitStatus> RunSilentAsync(
        string fileName,
        IEnumerable<string>? arguments,
        CancellationToken cancellationToken
    )
    {
        var startInfo = CreateStartInfo(fileName, arguments, true);

        using var process =
            Process.Start(startInfo)
            ?? throw new InvalidOperationException("The process could not be started.");

        try
        {
            // The streams must be drained, otherwise the process may block on a full pipe buffer
            await foreach (
                var _ in process.ReadBytesAsync(cancellationToken).ConfigureAwait(false)
            ) { }
        }
        // The process is killed below unless it has already exited
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { }
        catch
        {
            process.TryKill();
            throw;
        }

        return await process.WaitForExitStatusOrKillAsync(cancellationToken).ConfigureAwait(false);
    }
#endif

    private const int BufferSize = 4096;

    // Converts chunks of bytes to lines using the same line break rules as TextReader.ReadLine()
    // ("\n", "\r", or "\r\n"), while keeping the state that carries over between the chunks.
    private sealed class LineDecoder(Encoding encoding)
    {
        private readonly Decoder _decoder = encoding.GetDecoder();
        private readonly StringBuilder _line = new();
        private bool _isCarriageReturnPending;

        // An empty chunk means that the stream has ended, which flushes the decoder and the last
        // line (it doesn't have to be terminated by a line break).
        public IEnumerable<string> ReadLines(ArraySegment<byte> data)
        {
            var isEndOfStream = data.Count == 0;

            var chars = ArrayPool<char>.Shared.Rent(
                _decoder.GetCharCount(data.Array!, data.Offset, data.Count, isEndOfStream)
            );

            try
            {
                var charCount = _decoder.GetChars(
                    data.Array!,
                    data.Offset,
                    data.Count,
                    chars,
                    0,
                    isEndOfStream
                );

                for (var i = 0; i < charCount; i++)
                {
                    var ch = chars[i];

                    // Second half of a "\r\n" pair, which may have arrived in a previous chunk
                    if (_isCarriageReturnPending)
                    {
                        _isCarriageReturnPending = false;

                        if (ch == '\n')
                            continue;
                    }

                    if (ch == '\r' || ch == '\n')
                    {
                        yield return _line.ToString();

                        _line.Clear();
                        _isCarriageReturnPending = ch == '\r';
                    }
                    else
                    {
                        _line.Append(ch);
                    }
                }
            }
            finally
            {
                ArrayPool<char>.Shared.Return(chars);
            }

            if (isEndOfStream && _line.Length > 0)
            {
                yield return _line.ToString();

                _line.Clear();
            }
        }
    }

    extension(Process process)
    {
        private void TryKill()
        {
            try
            {
                process.Kill();
            }
            // Do our best and ignore race conditions (e.g. the process has already exited)
            catch { }
        }

        private ProcessExitStatus CreateExitStatus(bool isCanceled) =>
            new(
                process.ExitCode,
                isCanceled,
                // The process is forcibly killed on cancellation, which on Unix is done with SIGKILL
                isCanceled && !OperatingSystem.IsWindows()
                    ? (PosixSignal)9
                    : null
            );

        // Yields every chunk of data as soon as the underlying read releases, tagged with the stream
        // it came from. An empty chunk means that the stream has ended.
        // The buffer is only valid until the enumerator advances.
        // Once the enumerator is disposed, times out, or fails, the reading stops. A read that is
        // already in progress cannot be interrupted though, so it may still consume one more chunk.
        private IEnumerable<(ArraySegment<byte> Data, bool IsStandardError)> ReadBytes(
            int timeoutMilliseconds
        )
        {
            var stopwatch = Stopwatch.StartNew();

            var chunks =
                new BlockingCollection<(
                    ArraySegment<byte> Data,
                    bool IsStandardError,
                    Exception? Failure
                )>();

            var activePumps = 2;

            // Guards the flag, so that nothing is enqueued after the consumer has drained the queue
            var gate = new object();
            var isStopped = false;

            void Pump(Stream stream, bool isStandardError)
            {
                try
                {
                    int bytesRead;
                    do
                    {
                        // The pumps run ahead of the consumer, so a buffer can't be reused right away.
                        // Instead, it's handed over to the consumer, which returns it to the pool.
                        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);

                        bytesRead = stream.Read(buffer, 0, buffer.Length);

                        lock (gate)
                        {
                            // Nobody is going to consume the chunk
                            if (isStopped)
                            {
                                ArrayPool<byte>.Shared.Return(buffer);
                                return;
                            }

                            chunks.Add(
                                (
                                    new ArraySegment<byte>(buffer, 0, bytesRead),
                                    isStandardError,
                                    null
                                )
                            );
                        }
                    } while (bytesRead > 0);
                }
                catch (Exception ex)
                {
                    chunks.Add((default, isStandardError, ex));
                }
                finally
                {
                    // The last pump to finish marks the collection as complete
                    if (Interlocked.Decrement(ref activePumps) == 0)
                        chunks.CompleteAdding();
                }
            }

            var standardOutput = process.StandardOutput.BaseStream;
            var standardError = process.StandardError.BaseStream;

            // Redirected streams must be drained concurrently, otherwise the process may block
            // on a full pipe buffer of one stream while we're waiting on the other.
            new Thread(() => Pump(standardOutput, false)) { IsBackground = true }.Start();
            new Thread(() => Pump(standardError, true)) { IsBackground = true }.Start();

            var chunk = default((
                ArraySegment<byte> Data,
                bool IsStandardError,
                Exception? Failure
            ));

            try
            {
                while (
                    chunks.TryTake(
                        out chunk,
                        GetRemainingMilliseconds(stopwatch, timeoutMilliseconds)
                    )
                )
                {
                    // Fail as soon as any of the streams fails, because the other one may never end
                    if (chunk.Failure is not null)
                        throw chunk.Failure;

                    yield return (chunk.Data, chunk.IsStandardError);

                    // The consumer is done with the buffer once it asks for the next chunk
                    ArrayPool<byte>.Shared.Return(chunk.Data.Array!);
                    chunk = default;
                }

                // Taking fails either because all chunks were read or because of the timeout
                if (!chunks.IsCompleted)
                {
                    throw new TimeoutException(
                        "The redirected streams were not fully read within the specified timeout."
                    );
                }
            }
            finally
            {
                // The consumer is gone (disposed early, timed out, or failed), so tell the pumps to stop
                lock (gate)
                {
                    isStopped = true;
                }

                // The buffer that was handed to the consumer, but not returned yet
                if (chunk.Data.Array is not null)
                    ArrayPool<byte>.Shared.Return(chunk.Data.Array);

                // The buffers that were enqueued, but are never going to be consumed
                while (chunks.TryTake(out var pending, 0))
                {
                    if (pending.Data.Array is not null)
                        ArrayPool<byte>.Shared.Return(pending.Data.Array);
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.readallbytes
        public (byte[] StandardOutput, byte[] StandardError) ReadAllBytes(TimeSpan? timeout = null)
        {
            var timeoutMilliseconds = ToTimeoutMilliseconds(timeout);

            using var standardOutput = new MemoryStream();
            using var standardError = new MemoryStream();

            foreach (var (data, isStandardError) in process.ReadBytes(timeoutMilliseconds))
            {
                (isStandardError ? standardError : standardOutput).Write(
                    data.Array!,
                    data.Offset,
                    data.Count
                );
            }

            return (standardOutput.ToArray(), standardError.ToArray());
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.readalltext
        public (string StandardOutput, string StandardError) ReadAllText(TimeSpan? timeout = null)
        {
            var (outputBytes, errorBytes) = process.ReadAllBytes(timeout);

            return (
                process.StandardOutput.CurrentEncoding.GetString(outputBytes),
                process.StandardError.CurrentEncoding.GetString(errorBytes)
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.readalllines
        public IEnumerable<ProcessOutputLine> ReadAllLines(TimeSpan? timeout = null)
        {
            var timeoutMilliseconds = ToTimeoutMilliseconds(timeout);

            var standardOutputDecoder = new LineDecoder(process.StandardOutput.CurrentEncoding);
            var standardErrorDecoder = new LineDecoder(process.StandardError.CurrentEncoding);

            foreach (var (data, isStandardError) in process.ReadBytes(timeoutMilliseconds))
            {
                var decoder = isStandardError ? standardErrorDecoder : standardOutputDecoder;

                foreach (var line in decoder.ReadLines(data))
                    yield return new ProcessOutputLine(line, isStandardError);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.trywaitforexitstatus
        public bool TryWaitForExitStatus(
            TimeSpan timeout,
            [NotNullWhen(true)] out ProcessExitStatus? exitStatus
        )
        {
            if (!process.WaitForExit(ToTimeoutMilliseconds(timeout)))
            {
                exitStatus = null;
                return false;
            }

            exitStatus = process.CreateExitStatus(false);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexitstatus
        public ProcessExitStatus WaitForExitStatus()
        {
            process.TryWaitForExitStatus(Timeout.InfiniteTimeSpan, out var exitStatus);
            return exitStatus!;
        }

        // Waits for the process to exit, or kills it once the time is up (in which case the status
        // is marked as canceled)
        private ProcessExitStatus WaitForExitStatusOrKill(TimeSpan timeout)
        {
            if (process.TryWaitForExitStatus(timeout, out var exitStatus))
                return exitStatus;

            process.TryKill();
            process.WaitForExit();

            return process.CreateExitStatus(true);
        }
    }

    // Task infrastructure is required for async method support
#if FEATURE_TASK
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexitstatusasync
        public async Task<ProcessExitStatus> WaitForExitStatusAsync(
            CancellationToken cancellationToken = default
        )
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            return process.CreateExitStatus(false);
        }

        // Waits for the process to exit, or kills it once the cancellation is requested (in which
        // case the status is marked as canceled)
        private async Task<ProcessExitStatus> WaitForExitStatusOrKillAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                return await process
                    .WaitForExitStatusAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                process.TryKill();
                await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);

                return process.CreateExitStatus(true);
            }
        }

        // Async counterpart of ReadBytes(...). It doesn't have to run ahead of the consumer, so a single
        // buffer is reused per stream: the next read only starts once the previous chunk was consumed.
        // The buffer is only valid until the enumerator advances.
        private async IAsyncEnumerable<(
            ArraySegment<byte> Data,
            bool IsStandardError
        )> ReadBytesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var standardOutput = process.StandardOutput.BaseStream;
            var standardError = process.StandardError.BaseStream;

            var outputBuffer = new byte[BufferSize];
            var errorBuffer = new byte[BufferSize];

            // The pending reads, which are null once the respective stream has ended
            var outputRead = standardOutput.ReadAsync(outputBuffer, 0, outputBuffer.Length, cancellationToken);
            var errorRead = standardError.ReadAsync(errorBuffer, 0, errorBuffer.Length, cancellationToken);

            while (outputRead is not null || errorRead is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Wait until any of the pending reads completes. Stream reads are not always
                // cancellable, so make sure that cancellation is observed promptly.
                if (outputRead?.IsCompleted != true && errorRead?.IsCompleted != true)
                {
                    var pendingTask =
                        outputRead is not null && errorRead is not null
                            ? Task.WhenAny(outputRead, errorRead).Unwrap()
                            : (outputRead ?? errorRead)!;

                    await pendingTask.WaitAsync(cancellationToken).ConfigureAwait(false);
                }

                // Same as native: standard error takes precedence when both have data available.
                // Awaiting the completed read also fails as soon as any of the streams fails,
                // because the other one may never end.
                if (errorRead?.IsCompleted == true)
                {
                    var bytesRead = await errorRead.ConfigureAwait(false);
                    errorRead = null;

                    yield return (new ArraySegment<byte>(errorBuffer, 0, bytesRead), true);

                    // Keep reading until the stream ends (zero bytes)
                    if (bytesRead > 0)
                        errorRead = standardError.ReadAsync(errorBuffer, 0, errorBuffer.Length, cancellationToken);
                }
                else
                {
                    var bytesRead = await outputRead!.ConfigureAwait(false);
                    outputRead = null;

                    yield return (new ArraySegment<byte>(outputBuffer, 0, bytesRead), false);

                    if (bytesRead > 0)
                        outputRead = standardOutput.ReadAsync(outputBuffer, 0, outputBuffer.Length, cancellationToken);
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.readallbytesasync
        public async Task<(byte[] StandardOutput, byte[] StandardError)> ReadAllBytesAsync(
            CancellationToken cancellationToken = default
        )
        {
            using var standardOutput = new MemoryStream();
            using var standardError = new MemoryStream();

            await foreach (
                var (data, isStandardError) in process
                    .ReadBytesAsync(cancellationToken)
                    .ConfigureAwait(false)
            )
            {
                (isStandardError ? standardError : standardOutput).Write(
                    data.Array!,
                    data.Offset,
                    data.Count
                );
            }

            return (standardOutput.ToArray(), standardError.ToArray());
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.readalltextasync
        public async Task<(string StandardOutput, string StandardError)> ReadAllTextAsync(
            CancellationToken cancellationToken = default
        )
        {
            var (outputBytes, errorBytes) = await process
                .ReadAllBytesAsync(cancellationToken)
                .ConfigureAwait(false);

            return (
                process.StandardOutput.CurrentEncoding.GetString(outputBytes),
                process.StandardError.CurrentEncoding.GetString(errorBytes)
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.readalllinesasync
        public async IAsyncEnumerable<ProcessOutputLine> ReadAllLinesAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            var standardOutputDecoder = new LineDecoder(process.StandardOutput.CurrentEncoding);
            var standardErrorDecoder = new LineDecoder(process.StandardError.CurrentEncoding);

            await foreach (
                var (data, isStandardError) in process
                    .ReadBytesAsync(cancellationToken)
                    .ConfigureAwait(false)
            )
            {
                var decoder = isStandardError ? standardErrorDecoder : standardOutputDecoder;

                foreach (var line in decoder.ReadLines(data))
                    yield return new ProcessOutputLine(line, isStandardError);
            }
        }
    }
#endif

    extension(Process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.startandforget#system-diagnostics-process-startandforget(system-diagnostics-processstartinfo)
        public static int StartAndForget(ProcessStartInfo startInfo)
        {
            ArgumentNullException.ThrowIfNull(startInfo);

            if (startInfo.UseShellExecute)
            {
                throw new InvalidOperationException(
                    "StartAndForget does not support UseShellExecute. Set UseShellExecute to false."
                );
            }

            if (
                startInfo.RedirectStandardInput
                || startInfo.RedirectStandardOutput
                || startInfo.RedirectStandardError
            )
            {
                throw new InvalidOperationException(
                    "StartAndForget does not support stream redirection."
                );
            }

            // Unlike native, this doesn't redirect the standard handles to the null file by default,
            // because it's not possible to do that without access to the process handle APIs.
            using var process =
                Process.Start(startInfo)
                ?? throw new InvalidOperationException("The process could not be started.");

            return process.Id;
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.startandforget#system-diagnostics-process-startandforget(system-string-system-collections-generic-ienumerable((system-string)))
        public static int StartAndForget(string fileName, IEnumerable<string>? arguments = null) =>
            Process.StartAndForget(CreateStartInfo(fileName, arguments));

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.run#system-diagnostics-process-run(system-diagnostics-processstartinfo-system-nullable((system-timespan)))
        public static ProcessExitStatus Run(ProcessStartInfo startInfo, TimeSpan? timeout = null)
        {
            ArgumentNullException.ThrowIfNull(startInfo);

            if (startInfo.UseShellExecute)
            {
                throw new InvalidOperationException(
                    "Run does not support UseShellExecute. Set UseShellExecute to false."
                );
            }

            if (
                startInfo.RedirectStandardInput
                || startInfo.RedirectStandardOutput
                || startInfo.RedirectStandardError
            )
            {
                throw new InvalidOperationException(
                    "Run does not support stream redirection. Use RunAndCaptureText instead."
                );
            }

            // Validate before starting the process
            ToTimeoutMilliseconds(timeout);

            using var process =
                Process.Start(startInfo)
                ?? throw new InvalidOperationException("The process could not be started.");

            return process.WaitForExitStatusOrKill(timeout ?? Timeout.InfiniteTimeSpan);
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.run#system-diagnostics-process-run(system-string-system-collections-generic-ienumerable((system-string))-system-boolean-system-nullable((system-timespan)))
        public static ProcessExitStatus Run(
            string fileName,
            IEnumerable<string>? arguments = null,
            bool silent = false,
            TimeSpan? timeout = null
        ) =>
            // Unlike native, silent mode doesn't redirect the standard handles to the null device,
            // because it's not possible to do that without access to the process handle APIs.
            // Instead, the output is redirected, drained, and discarded, and the standard input is
            // inherited.
            silent
                ? RunSilent(fileName, arguments, timeout)
                : Process.Run(CreateStartInfo(fileName, arguments), timeout);

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.runandcapturetext#system-diagnostics-process-runandcapturetext(system-diagnostics-processstartinfo-system-nullable((system-timespan)))
        public static ProcessTextOutput RunAndCaptureText(
            ProcessStartInfo startInfo,
            TimeSpan? timeout = null
        )
        {
            ArgumentNullException.ThrowIfNull(startInfo);

            if (startInfo.UseShellExecute)
            {
                throw new InvalidOperationException(
                    "RunAndCaptureText does not support UseShellExecute. Set UseShellExecute to false."
                );
            }

            if (!startInfo.RedirectStandardOutput || !startInfo.RedirectStandardError)
            {
                throw new InvalidOperationException(
                    "RunAndCaptureText requires both RedirectStandardOutput and RedirectStandardError to be set to true."
                );
            }

            // Validate before starting the process
            var timeoutMilliseconds = ToTimeoutMilliseconds(timeout);

            var stopwatch = Stopwatch.StartNew();

            using var process =
                Process.Start(startInfo)
                ?? throw new InvalidOperationException("The process could not be started.");

            var processId = process.Id;

            string standardOutput;
            string standardError;
            try
            {
                (standardOutput, standardError) = process.ReadAllText(timeout);
            }
            catch
            {
                process.TryKill();
                throw;
            }

            var isCanceled = false;
            if (!process.WaitForExit(GetRemainingMilliseconds(stopwatch, timeoutMilliseconds)))
            {
                process.TryKill();
                process.WaitForExit();
                isCanceled = true;
            }

            return new ProcessTextOutput(
                process.CreateExitStatus(isCanceled),
                standardOutput,
                standardError,
                processId
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.runandcapturetext#system-diagnostics-process-runandcapturetext(system-string-system-collections-generic-ienumerable((system-string))-system-nullable((system-timespan)))
        public static ProcessTextOutput RunAndCaptureText(
            string fileName,
            IEnumerable<string>? arguments = null,
            TimeSpan? timeout = null
        ) => Process.RunAndCaptureText(CreateStartInfo(fileName, arguments, true), timeout);

        // Task infrastructure is required for async method support
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.runasync#system-diagnostics-process-runasync(system-diagnostics-processstartinfo-system-threading-cancellationtoken)
        public static async Task<ProcessExitStatus> RunAsync(
            ProcessStartInfo startInfo,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(startInfo);

            if (startInfo.UseShellExecute)
            {
                throw new InvalidOperationException(
                    "RunAsync does not support UseShellExecute. Set UseShellExecute to false."
                );
            }

            if (
                startInfo.RedirectStandardInput
                || startInfo.RedirectStandardOutput
                || startInfo.RedirectStandardError
            )
            {
                throw new InvalidOperationException(
                    "RunAsync does not support stream redirection. Use RunAndCaptureTextAsync instead."
                );
            }

            using var process =
                Process.Start(startInfo)
                ?? throw new InvalidOperationException("The process could not be started.");

            return await process
                .WaitForExitStatusOrKillAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.runasync#system-diagnostics-process-runasync(system-string-system-collections-generic-ienumerable((system-string))-system-boolean-system-threading-cancellationtoken)
        public static async Task<ProcessExitStatus> RunAsync(
            string fileName,
            IEnumerable<string>? arguments = null,
            bool silent = false,
            CancellationToken cancellationToken = default
        ) =>
            // See the note on the synchronous overload about silent mode
            silent
                ? await RunSilentAsync(fileName, arguments, cancellationToken).ConfigureAwait(false)
                : await Process
                    .RunAsync(CreateStartInfo(fileName, arguments), cancellationToken)
                    .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.runandcapturetextasync#system-diagnostics-process-runandcapturetextasync(system-diagnostics-processstartinfo-system-threading-cancellationtoken)
        public static async Task<ProcessTextOutput> RunAndCaptureTextAsync(
            ProcessStartInfo startInfo,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(startInfo);

        if (startInfo.UseShellExecute)
        {
            throw new InvalidOperationException(
                "RunAndCaptureTextAsync does not support UseShellExecute. Set UseShellExecute to false."
            );
        }

        if (!startInfo.RedirectStandardOutput || !startInfo.RedirectStandardError)
        {
            throw new InvalidOperationException(
                "RunAndCaptureTextAsync requires both RedirectStandardOutput and RedirectStandardError to be set to true."
            );
        }

            using var process =
                Process.Start(startInfo)
                ?? throw new InvalidOperationException("The process could not be started.");

            var processId = process.Id;

            string standardOutput;
            string standardError;
            try
            {
                (standardOutput, standardError) = await process
                    .ReadAllTextAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch
            {
                process.TryKill();
                throw;
            }

            var isCanceled = false;
            try
            {
                await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                isCanceled = true;
            }

            if (isCanceled)
            {
                process.TryKill();
                await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            }

            return new ProcessTextOutput(
                process.CreateExitStatus(isCanceled),
                standardOutput,
                standardError,
                processId
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.runandcapturetextasync#system-diagnostics-process-runandcapturetextasync(system-string-system-collections-generic-ienumerable((system-string))-system-threading-cancellationtoken)
        public static async Task<ProcessTextOutput> RunAndCaptureTextAsync(
            string fileName,
            IEnumerable<string>? arguments = null,
            CancellationToken cancellationToken = default
        ) =>
            await Process.RunAndCaptureTextAsync(
                CreateStartInfo(fileName, arguments, true),
                cancellationToken
            );
#endif

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.trygetprocessbyid
        public static bool TryGetProcessById(
            int processId,
            [NotNullWhen(true)] out Process? process
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(processId);

            try
            {
                process = Process.GetProcessById(processId);
                return true;
            }
            // Thrown when the process is not running
            catch (ArgumentException)
            {
                process = null;
                return false;
            }
        }
    }
}
#endif
#endif
