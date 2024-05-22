﻿#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.requiresunreferencedcodeattribute
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method,
    Inherited = false
)]
[ExcludeFromCodeCoverage]
internal class RequiresUnreferencedCodeAttribute(string message) : Attribute
{
    public string Message { get; } = message;

    public string? Url { get; init; }
}
#endif
