#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.systemexception
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class SystemException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
#endif
