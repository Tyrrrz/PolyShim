using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class ProcessTests
{
    private static ProcessStartInfo CreateStartInfo(
        string unixScript,
        string windowsScript,
        bool redirect = true
    )
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = OperatingSystem.IsWindows() ? "cmd" : "sh",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = redirect,
            RedirectStandardError = redirect,
        };

        startInfo.ArgumentList.Add(OperatingSystem.IsWindows() ? "/c" : "-c");
        startInfo.ArgumentList.Add(OperatingSystem.IsWindows() ? windowsScript : unixScript);

        return startInfo;
    }

    // Process that writes to both streams and exits
    private static ProcessStartInfo CreateOutputStartInfo(bool redirect = true) =>
        CreateStartInfo("echo out; echo err 1>&2", "echo out& 1>&2 echo err", redirect);

    // Process that doesn't exit (and keeps the redirected streams open) for a long time
    private static ProcessStartInfo CreateHangingStartInfo() =>
        CreateStartInfo("exec sleep 30", "ping -n 30 127.0.0.1");

    private static string HangingScript =>
        OperatingSystem.IsWindows() ? "ping -n 30 127.0.0.1" : "exec sleep 30";

    private static Process StartProcess(ProcessStartInfo startInfo) =>
        Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start.");

    private static void KillSilently(Process process)
    {
        try
        {
            process.Kill();
        }
        catch
        {
            // Ignore
        }
    }

    [Fact]
    public void StartAndForget_StartInfo_Test()
    {
        // Act
        var processId = Process.StartAndForget(CreateStartInfo("exit 0", "exit 0", false));

        // Assert
        processId.Should().BePositive();
    }

    [Fact]
    public void StartAndForget_FileName_Test()
    {
        // Act
        var processId = Process.StartAndForget(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "exit 0"]
        );

        // Assert
        processId.Should().BePositive();
    }

    [Fact]
    public void StartAndForget_StartInfo_Null_Test()
    {
        // Act & assert
        var act = () => Process.StartAndForget((ProcessStartInfo)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void StartAndForget_StartInfo_ShellExecute_Test()
    {
        // Arrange
        var startInfo = CreateStartInfo("exit 0", "exit 0", false);
        startInfo.UseShellExecute = true;

        // Act & assert
        var act = () => Process.StartAndForget(startInfo);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void StartAndForget_StartInfo_Redirected_Test()
    {
        // Act & assert
        var act = () => Process.StartAndForget(CreateStartInfo("exit 0", "exit 0"));
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void StartAndForget_FileName_Null_Test()
    {
        // Act & assert
        var act = () => Process.StartAndForget((string)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void StartAndForget_FileName_Empty_Test()
    {
        // Act & assert
        var act = () => Process.StartAndForget("");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TryGetProcessById_Test()
    {
        // Act
        var found = Process.TryGetProcessById(Environment.ProcessId, out var process);

        // Assert
        found.Should().BeTrue();
        process.Should().NotBeNull();
        process!.Id.Should().Be(Environment.ProcessId);

        process.Dispose();
    }

    [Fact]
    public void TryGetProcessById_NotFound_Test()
    {
        // Act
        var found = Process.TryGetProcessById(int.MaxValue, out var process);

        // Assert
        found.Should().BeFalse();
        process.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TryGetProcessById_Invalid_Test(int processId)
    {
        // Act & assert
        var act = () => Process.TryGetProcessById(processId, out _);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ReadAllBytes_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo());

        // Act
        var (standardOutput, standardError) = process.ReadAllBytes();

        // Assert
        Encoding.UTF8.GetString(standardOutput).Trim().Should().Be("out");
        Encoding.UTF8.GetString(standardError).Trim().Should().Be("err");
    }

    [SkippableFact]
    public void ReadAllBytes_Large_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // Both streams have to be drained concurrently, otherwise the process blocks
        using var process = StartProcess(
            CreateStartInfo("head -c 500000 /dev/zero; head -c 500000 /dev/zero 1>&2", string.Empty)
        );

        // Act
        var (standardOutput, standardError) = process.ReadAllBytes(TimeSpan.FromSeconds(30));

        // Assert
        standardOutput.Should().HaveCount(500000);
        standardError.Should().HaveCount(500000);
    }

    [Fact]
    public void ReadAllBytes_NotRedirected_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo(false));

        // Act & assert
        var act = () => process.ReadAllBytes();
        act.Should().Throw<InvalidOperationException>();

        process.WaitForExit();
    }

    [Fact]
    public void ReadAllBytes_Timeout_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());

        try
        {
            // Act & assert
            var act = () => process.ReadAllBytes(TimeSpan.FromMilliseconds(500));
            act.Should().Throw<TimeoutException>();
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public void ReadAllBytes_StreamFailure_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());

        try
        {
            // The other stream stays open, so it must not be waited on after this one fails
            process.StandardOutput.Dispose();

            // Act & assert
            var stopwatch = Stopwatch.StartNew();
            var act = () => process.ReadAllBytes();
            act.Should().Throw<Exception>();

            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(15));
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public void ReadAllBytes_InvalidTimeout_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo());

        // Act & assert
        var act = () => process.ReadAllBytes(TimeSpan.FromMilliseconds(-2));
        act.Should().Throw<ArgumentOutOfRangeException>();

        process.WaitForExit();
    }

    [Fact]
    public void ReadAllText_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo());

        // Act
        var (standardOutput, standardError) = process.ReadAllText();

        // Assert
        standardOutput.Trim().Should().Be("out");
        standardError.Trim().Should().Be("err");
    }

    [Fact]
    public void ReadAllLines_Test()
    {
        // Arrange
        using var process = StartProcess(
            CreateStartInfo(
                "echo out1; echo out2; echo err1 1>&2; echo err2 1>&2",
                "echo out1& echo out2& 1>&2 echo err1& 1>&2 echo err2"
            )
        );

        // Act
        var lines = process.ReadAllLines().ToArray();

        // Assert
        lines
            .Where(l => !l.StandardError)
            .Select(l => l.Content.Trim())
            .Should()
            .Equal("out1", "out2");

        lines
            .Where(l => l.StandardError)
            .Select(l => l.Content.Trim())
            .Should()
            .Equal("err1", "err2");
    }

    [SkippableFact]
    public void ReadAllLines_LineBreaks_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // Same line break rules as TextReader.ReadLine(): "\r", "\n", "\r\n", no trailing empty line
        using var process = StartProcess(
            CreateStartInfo("printf 'a\\rb\\r\\nc\\n\\nd'", string.Empty)
        );

        // Act
        var lines = process.ReadAllLines().Where(l => !l.StandardError).Select(l => l.Content);

        // Assert
        lines.Should().Equal("a", "b", "c", "", "d");
    }

    [SkippableFact]
    public void ReadAllLines_LineBreaks_Split_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // "\r\n" may be split across separate reads, which must still produce a single line break
        using var process = StartProcess(
            CreateStartInfo("printf 'a\\r'; sleep 1; printf '\\nb\\n'", string.Empty)
        );

        // Act
        var lines = process.ReadAllLines().Where(l => !l.StandardError).Select(l => l.Content);

        // Assert
        lines.Should().Equal("a", "b");
    }

    [SkippableFact]
    public void ReadAllLines_MultiByte_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // Multi-byte characters may be split across separate reads (octal escapes, as POSIX printf has no \x).
        // The output is longer than 4 bytes because native doesn't split lines on shorter outputs.
        using var process = StartProcess(
            CreateStartInfo("printf 'abc\\303'; sleep 1; printf '\\251\\n'", string.Empty)
        );

        // Act
        var lines = process.ReadAllLines().Where(l => !l.StandardError).Select(l => l.Content);

        // Assert
        lines.Should().Equal("abc\u00e9");
    }

    [Fact]
    public void ReadAllLines_NotRedirected_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo(false));

        // Act & assert
        var act = () => process.ReadAllLines().ToArray();
        act.Should().Throw<InvalidOperationException>();

        process.WaitForExit();
    }

    [Fact]
    public void ReadAllLines_Timeout_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());

        try
        {
            // Act & assert
            var act = () => process.ReadAllLines(TimeSpan.FromMilliseconds(500)).ToArray();
            act.Should().Throw<TimeoutException>();
        }
        finally
        {
            KillSilently(process);
        }
    }

    [SkippableFact]
    public void ReadAllLines_EarlyDisposal_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        using var process = StartProcess(CreateStartInfo("exec yes", ""));

        try
        {
            // Act
            // Taking the first line disposes the enumerator while the process keeps writing
            var line = process.ReadAllLines().First();

            // Assert
            line.Content.Should().Be("y");
            line.StandardError.Should().BeFalse();
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public void ReadAllLines_StreamFailure_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());

        try
        {
            // The other stream stays open, so it must not be waited on after this one fails
            process.StandardOutput.Dispose();

            // Act & assert
            var stopwatch = Stopwatch.StartNew();
            var act = () => process.ReadAllLines().ToArray();
            act.Should().Throw<Exception>();

            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(15));
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public async Task ReadAllBytesAsync_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo());

        // Act
        var (standardOutput, standardError) = await process.ReadAllBytesAsync();

        // Assert
        Encoding.UTF8.GetString(standardOutput).Trim().Should().Be("out");
        Encoding.UTF8.GetString(standardError).Trim().Should().Be("err");
    }

    [Fact]
    public async Task ReadAllBytesAsync_NotRedirected_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo(false));

        // Act & assert
        var act = async () => await process.ReadAllBytesAsync();
        await act.Should().ThrowAsync<InvalidOperationException>();

        process.WaitForExit();
    }

    [Fact]
    public async Task ReadAllBytesAsync_Cancellation_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        try
        {
            // Act & assert
            var act = async () => await process.ReadAllBytesAsync(cts.Token);
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public async Task ReadAllTextAsync_Test()
    {
        // Arrange
        using var process = StartProcess(CreateOutputStartInfo());

        // Act
        var (standardOutput, standardError) = await process.ReadAllTextAsync();

        // Assert
        standardOutput.Trim().Should().Be("out");
        standardError.Trim().Should().Be("err");
    }

    [Fact]
    public async Task ReadAllLinesAsync_Test()
    {
        // Arrange
        using var process = StartProcess(
            CreateStartInfo(
                "echo out1; echo out2; echo err1 1>&2; echo err2 1>&2",
                "echo out1& echo out2& 1>&2 echo err1& 1>&2 echo err2"
            )
        );

        // Act
        var lines = new List<ProcessOutputLine>();
        await foreach (var line in process.ReadAllLinesAsync())
            lines.Add(line);

        // Assert
        lines
            .Where(l => !l.StandardError)
            .Select(l => l.Content.Trim())
            .Should()
            .Equal("out1", "out2");

        lines
            .Where(l => l.StandardError)
            .Select(l => l.Content.Trim())
            .Should()
            .Equal("err1", "err2");
    }

    [SkippableFact]
    public async Task ReadAllBytesAsync_Large_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // Both streams have to be drained concurrently, otherwise the process blocks
        using var process = StartProcess(
            CreateStartInfo("head -c 500000 /dev/zero; head -c 500000 /dev/zero 1>&2", string.Empty)
        );

        // Act
        var (standardOutput, standardError) = await process.ReadAllBytesAsync();

        // Assert
        standardOutput.Should().HaveCount(500000);
        standardError.Should().HaveCount(500000);
    }

    [Fact]
    public async Task ReadAllBytesAsync_StreamFailure_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());

        try
        {
            // The other stream stays open, so it must not be waited on after this one fails
            process.StandardOutput.Dispose();

            // Act & assert
            var stopwatch = Stopwatch.StartNew();
            var act = async () => await process.ReadAllBytesAsync();
            await act.Should().ThrowAsync<Exception>();

            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(15));
        }
        finally
        {
            KillSilently(process);
        }
    }

    [SkippableFact]
    public async Task ReadAllLinesAsync_LineBreaks_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // Same line break rules as TextReader.ReadLine(): "\r", "\n", "\r\n", no trailing empty line
        using var process = StartProcess(
            CreateStartInfo("printf 'a\\rb\\r\\nc\\n\\nd'", string.Empty)
        );

        // Act
        var lines = new List<string>();
        await foreach (var line in process.ReadAllLinesAsync())
        {
            if (!line.StandardError)
                lines.Add(line.Content);
        }

        // Assert
        lines.Should().Equal("a", "b", "c", "", "d");
    }

    [SkippableFact]
    public async Task ReadAllLinesAsync_LineBreaks_Split_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // "\r\n" may be split across separate reads, which must still produce a single line break
        using var process = StartProcess(
            CreateStartInfo("printf 'a\\r'; sleep 1; printf '\\nb\\n'", string.Empty)
        );

        // Act
        var lines = new List<string>();
        await foreach (var line in process.ReadAllLinesAsync())
        {
            if (!line.StandardError)
                lines.Add(line.Content);
        }

        // Assert
        lines.Should().Equal("a", "b");
    }

    [SkippableFact]
    public async Task ReadAllLinesAsync_MultiByte_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // Multi-byte characters may be split across separate reads (octal escapes, as POSIX printf has no \x).
        // The output is longer than 4 bytes because native doesn't split lines on shorter outputs.
        using var process = StartProcess(
            CreateStartInfo("printf 'abc\\303'; sleep 1; printf '\\251\\n'", string.Empty)
        );

        // Act
        var lines = new List<string>();
        await foreach (var line in process.ReadAllLinesAsync())
        {
            if (!line.StandardError)
                lines.Add(line.Content);
        }

        // Assert
        lines.Should().Equal("abc\u00e9");
    }

    [Fact]
    public async Task ReadAllLinesAsync_Cancellation_Test()
    {
        // Arrange
        using var process = StartProcess(CreateHangingStartInfo());
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        try
        {
            // Act & assert
            var act = async () =>
            {
                await foreach (var _ in process.ReadAllLinesAsync(cts.Token)) { }
            };

            await act.Should().ThrowAsync<OperationCanceledException>();
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public void RunAndCaptureText_StartInfo_Test()
    {
        // Act
        var output = Process.RunAndCaptureText(CreateOutputStartInfo());

        // Assert
        output.StandardOutput.Trim().Should().Be("out");
        output.StandardError.Trim().Should().Be("err");
        output.ExitStatus.ExitCode.Should().Be(0);
        output.ExitStatus.Canceled.Should().BeFalse();
        output.ProcessId.Should().BePositive();
    }

    [Fact]
    public void RunAndCaptureText_FileName_Test()
    {
        // Act
        var output = Process.RunAndCaptureText(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "echo out"]
        );

        // Assert
        output.StandardOutput.Trim().Should().Be("out");
        output.StandardError.Should().BeEmpty();
        output.ExitStatus.ExitCode.Should().Be(0);
    }

    [Fact]
    public void RunAndCaptureText_ExitCode_Test()
    {
        // Act
        var output = Process.RunAndCaptureText(CreateStartInfo("exit 3", "exit 3"));

        // Assert
        output.ExitStatus.ExitCode.Should().Be(3);
        output.ExitStatus.Canceled.Should().BeFalse();
    }

    [Fact]
    public void RunAndCaptureText_NotRedirected_Test()
    {
        // Act & assert
        var act = () => Process.RunAndCaptureText(CreateOutputStartInfo(false));
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RunAndCaptureText_ShellExecute_Test()
    {
        // Arrange
        var startInfo = CreateOutputStartInfo();
        startInfo.UseShellExecute = true;

        // Act & assert
        var act = () => Process.RunAndCaptureText(startInfo);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RunAndCaptureText_Null_Test()
    {
        // Act & assert
        var act = () => Process.RunAndCaptureText((ProcessStartInfo)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RunAndCaptureText_FileName_Empty_Test()
    {
        // Act & assert
        var act = () => Process.RunAndCaptureText("");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RunAndCaptureText_Timeout_Test()
    {
        // Act & assert
        var act = () =>
            Process.RunAndCaptureText(CreateHangingStartInfo(), TimeSpan.FromMilliseconds(500));

        act.Should().Throw<TimeoutException>();
    }

    [SkippableFact]
    public void RunAndCaptureText_Timeout_Exit_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // The streams are closed right away, but the process itself keeps running
        var startInfo = CreateStartInfo("exec >&- 2>&-; exec sleep 30", string.Empty);

        // Act
        var output = Process.RunAndCaptureText(startInfo, TimeSpan.FromMilliseconds(500));

        // Assert
        output.ExitStatus.Canceled.Should().BeTrue();
    }

    [Fact]
    public async Task RunAndCaptureTextAsync_StartInfo_Test()
    {
        // Act
        var output = await Process.RunAndCaptureTextAsync(CreateOutputStartInfo());

        // Assert
        output.StandardOutput.Trim().Should().Be("out");
        output.StandardError.Trim().Should().Be("err");
        output.ExitStatus.ExitCode.Should().Be(0);
        output.ExitStatus.Canceled.Should().BeFalse();
        output.ProcessId.Should().BePositive();
    }

    [Fact]
    public async Task RunAndCaptureTextAsync_FileName_Test()
    {
        // Act
        var output = await Process.RunAndCaptureTextAsync(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "echo out"]
        );

        // Assert
        output.StandardOutput.Trim().Should().Be("out");
        output.StandardError.Should().BeEmpty();
        output.ExitStatus.ExitCode.Should().Be(0);
    }

    [Fact]
    public async Task RunAndCaptureTextAsync_ExitCode_Test()
    {
        // Act
        var output = await Process.RunAndCaptureTextAsync(CreateStartInfo("exit 3", "exit 3"));

        // Assert
        output.ExitStatus.ExitCode.Should().Be(3);
        output.ExitStatus.Canceled.Should().BeFalse();
    }

    [Fact]
    public async Task RunAndCaptureTextAsync_NotRedirected_Test()
    {
        // Act & assert
        var act = async () => await Process.RunAndCaptureTextAsync(CreateOutputStartInfo(false));
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RunAndCaptureTextAsync_FileName_Empty_Test()
    {
        // Act & assert
        var act = async () => await Process.RunAndCaptureTextAsync("");
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task RunAndCaptureTextAsync_Cancellation_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        // Act & assert
        var act = async () =>
            await Process.RunAndCaptureTextAsync(CreateHangingStartInfo(), cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [SkippableFact]
    public async Task RunAndCaptureTextAsync_Cancellation_Exit_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        // The streams are closed right away, but the process itself keeps running
        var startInfo = CreateStartInfo("exec >&- 2>&-; exec sleep 30", string.Empty);
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        // Act
        var output = await Process.RunAndCaptureTextAsync(startInfo, cts.Token);

        // Assert
        output.ExitStatus.Canceled.Should().BeTrue();
    }

    [Fact]
    public void Run_StartInfo_Test()
    {
        // Act
        var status = Process.Run(CreateStartInfo("exit 3", "exit 3", false));

        // Assert
        status.ExitCode.Should().Be(3);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public void Run_FileName_Test()
    {
        // Act
        var status = Process.Run(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "exit 2"]
        );

        // Assert
        status.ExitCode.Should().Be(2);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public void Run_FileName_Silent_Test()
    {
        // Act
        var status = Process.Run(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "echo out& exit 4"],
            silent: true
        );

        // Assert
        status.ExitCode.Should().Be(4);
    }

    [Fact]
    public void Run_FileName_Silent_Timeout_Test()
    {
        // The process keeps the redirected streams open, so draining them never completes
        // Act
        var status = Process.Run(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", HangingScript],
            silent: true,
            timeout: TimeSpan.FromMilliseconds(500)
        );

        // Assert
        status.Canceled.Should().BeTrue();
    }

    [Fact]
    public void Run_Null_Test()
    {
        // Act & assert
        var act = () => Process.Run((ProcessStartInfo)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Run_ShellExecute_Test()
    {
        // Arrange
        var startInfo = CreateStartInfo("exit 0", "exit 0", false);
        startInfo.UseShellExecute = true;
        startInfo.RedirectStandardOutput = false;
        startInfo.RedirectStandardError = false;

        // Act & assert
        var act = () => Process.Run(startInfo);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Run_Redirected_Test()
    {
        // Act & assert
        var act = () => Process.Run(CreateOutputStartInfo());
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Run_FileName_Empty_Test()
    {
        // Act & assert
        var act = () => Process.Run("");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Run_Timeout_Invalid_Test()
    {
        // Act & assert
        var act = () =>
            Process.Run(CreateStartInfo("exit 0", "exit 0", false), TimeSpan.FromMilliseconds(-2));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Run_Timeout_Test()
    {
        // Act
        var status = Process.Run(
            CreateStartInfo("exec sleep 30", "ping -n 30 127.0.0.1", false),
            TimeSpan.FromMilliseconds(500)
        );

        // Assert
        status.Canceled.Should().BeTrue();
    }

    [Fact]
    public void Run_Timeout_Exit_Test()
    {
        // Act
        var status = Process.Run(
            CreateStartInfo("exit 5", "exit 5", false),
            TimeSpan.FromSeconds(30)
        );

        // Assert
        status.ExitCode.Should().Be(5);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public async Task RunAsync_StartInfo_Test()
    {
        // Act
        var status = await Process.RunAsync(CreateStartInfo("exit 3", "exit 3", false));

        // Assert
        status.ExitCode.Should().Be(3);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public async Task RunAsync_FileName_Test()
    {
        // Act
        var status = await Process.RunAsync(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "exit 2"]
        );

        // Assert
        status.ExitCode.Should().Be(2);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public async Task RunAsync_FileName_Silent_Test()
    {
        // Act
        var status = await Process.RunAsync(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", "echo out& exit 4"],
            silent: true
        );

        // Assert
        status.ExitCode.Should().Be(4);
    }

    [Fact]
    public async Task RunAsync_FileName_Silent_Cancellation_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        // Act
        var status = await Process.RunAsync(
            OperatingSystem.IsWindows() ? "cmd" : "sh",
            [OperatingSystem.IsWindows() ? "/c" : "-c", HangingScript],
            silent: true,
            cancellationToken: cts.Token
        );

        // Assert
        status.Canceled.Should().BeTrue();
    }

    [Fact]
    public async Task RunAsync_Null_Test()
    {
        // Act & assert
        var act = async () => await Process.RunAsync((ProcessStartInfo)null!);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task RunAsync_Redirected_Test()
    {
        // Act & assert
        var act = async () => await Process.RunAsync(CreateOutputStartInfo());
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RunAsync_FileName_Empty_Test()
    {
        // Act & assert
        var act = async () => await Process.RunAsync("");
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task RunAsync_Cancellation_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

        // Act
        var status = await Process.RunAsync(
            CreateStartInfo("exec sleep 30", "ping -n 30 127.0.0.1", false),
            cts.Token
        );

        // Assert
        status.Canceled.Should().BeTrue();
    }

    [Fact]
    public void WaitForExitStatus_Test()
    {
        // Arrange
        using var process = StartProcess(CreateStartInfo("exit 3", "exit 3", false));

        // Act
        var status = process.WaitForExitStatus();

        // Assert
        status.ExitCode.Should().Be(3);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public void TryWaitForExitStatus_Test()
    {
        // Arrange
        using var process = StartProcess(CreateStartInfo("exit 3", "exit 3", false));

        // Act
        var exited = process.TryWaitForExitStatus(TimeSpan.FromSeconds(30), out var status);

        // Assert
        exited.Should().BeTrue();
        status!.ExitCode.Should().Be(3);
    }

    [Fact]
    public void TryWaitForExitStatus_Timeout_Test()
    {
        // Arrange
        using var process = StartProcess(
            CreateStartInfo("exec sleep 30", "ping -n 30 127.0.0.1", false)
        );

        try
        {
            // Act
            var exited = process.TryWaitForExitStatus(
                TimeSpan.FromMilliseconds(200),
                out var status
            );

            // Assert
            exited.Should().BeFalse();
            status.Should().BeNull();
            process.HasExited.Should().BeFalse();
        }
        finally
        {
            KillSilently(process);
        }
    }

    [Fact]
    public void TryWaitForExitStatus_Timeout_Invalid_Test()
    {
        // Arrange
        using var process = StartProcess(CreateStartInfo("exit 0", "exit 0", false));

        // Act & assert
        var act = () => process.TryWaitForExitStatus(TimeSpan.FromMilliseconds(-2), out _);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task WaitForExitStatusAsync_Test()
    {
        // Arrange
        using var process = StartProcess(CreateStartInfo("exit 3", "exit 3", false));

        // Act
        var status = await process.WaitForExitStatusAsync();

        // Assert
        status.ExitCode.Should().Be(3);
        status.Canceled.Should().BeFalse();
    }

    [Fact]
    public async Task WaitForExitStatusAsync_Cancellation_Test()
    {
        // Arrange
        using var process = StartProcess(
            CreateStartInfo("exec sleep 30", "ping -n 30 127.0.0.1", false)
        );
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        try
        {
            // Act & assert
            var act = async () => await process.WaitForExitStatusAsync(cts.Token);
            await act.Should().ThrowAsync<OperationCanceledException>();

            // The process is not killed on cancellation
            process.HasExited.Should().BeFalse();
        }
        finally
        {
            KillSilently(process);
        }
    }
}
