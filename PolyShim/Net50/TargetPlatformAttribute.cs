#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.targetplatformattribute
[AttributeUsage(AttributeTargets.Assembly)]
[ExcludeFromCodeCoverage]
internal class TargetPlatformAttribute(string platformName) : OSPlatformAttribute(platformName);
#endif
