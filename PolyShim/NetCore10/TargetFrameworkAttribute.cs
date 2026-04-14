#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.targetframeworkattribute
[AttributeUsage(AttributeTargets.Assembly)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class TargetFrameworkAttribute(string frameworkName) : Attribute
{
    public string FrameworkName { get; } = frameworkName;

    public string? FrameworkDisplayName { get; set; }
}
#endif
