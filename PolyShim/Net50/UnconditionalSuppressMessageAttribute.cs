﻿// This file was generated by the PolyShim library.
// <auto-generated/>

#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.unconditionalsuppressmessageattribute
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
[ExcludeFromCodeCoverage]
internal class UnconditionalSuppressMessageAttribute(string category, string checkId) : Attribute
{
    public string Category { get; } = category;

    public string CheckId { get; } = checkId;

    public string? MessageId { get; init; }

    public string? Justification { get; init; }

    public string? Scope { get; init; }

    public string? Target { get; init; }
}
#endif
