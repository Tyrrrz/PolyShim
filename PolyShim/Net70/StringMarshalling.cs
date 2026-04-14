#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.stringmarshalling
internal enum StringMarshalling
{
    Custom = 0,
    Utf8,
    Utf16,
}
#endif
