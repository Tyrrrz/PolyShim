#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// ProcessExitStatus is a part of the System.Diagnostics.Process assembly
#if FEATURE_PROCESS
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.processexitstatus
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ProcessExitStatus(int exitCode, bool canceled, PosixSignal? signal = null)
{
    public int ExitCode { get; } = exitCode;

    public PosixSignal? Signal { get; } = signal;

    public bool Canceled { get; } = canceled;
}
#endif
#endif
