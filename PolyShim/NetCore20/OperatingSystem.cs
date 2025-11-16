#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.operatingsystem
[ExcludeFromCodeCoverage]
internal class OperatingSystem;
// This should include members, but currently this polyfill is incomplete and
// serves only as a placeholder so that polyfills for static methods have a type to extend.
#endif
