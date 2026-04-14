#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.targetplatformattribute
[AttributeUsage(AttributeTargets.Assembly)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class TargetPlatformAttribute(string platformName) : OSPlatformAttribute(platformName);
#endif
