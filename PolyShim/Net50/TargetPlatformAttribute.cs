#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.targetplatformattribute
[AttributeUsage(AttributeTargets.Assembly)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class TargetPlatformAttribute(string platformName) : OSPlatformAttribute(platformName);
#endif
