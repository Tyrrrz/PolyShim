#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// ProcessOutputLine is a part of the System.Diagnostics.Process assembly
#if FEATURE_PROCESS
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.processoutputline
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ProcessOutputLine(string content, bool standardError)
{
    public string Content { get; } = content ?? throw new ArgumentNullException(nameof(content));

    public bool StandardError { get; } = standardError;

    public override string ToString() => Content ?? string.Empty;
}
#endif
#endif
