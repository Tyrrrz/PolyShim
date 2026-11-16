#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// ProcessTextOutput is a part of the System.Diagnostics.Process assembly
#if FEATURE_PROCESS
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.processtextoutput
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ProcessTextOutput(
    ProcessExitStatus exitStatus,
    string standardOutput,
    string standardError,
    int processId
)
{
    public ProcessExitStatus ExitStatus { get; } =
        exitStatus ?? throw new ArgumentNullException(nameof(exitStatus));

    public string StandardOutput { get; } =
        standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));

    public string StandardError { get; } =
        standardError ?? throw new ArgumentNullException(nameof(standardError));

    public int ProcessId { get; } = processId;
}
#endif
#endif
