#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.stringmarshalling
internal enum StringMarshalling
{
    Custom = 0,
    Utf8,
    Utf16
}
#endif