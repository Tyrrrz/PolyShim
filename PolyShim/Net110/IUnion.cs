#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Runtime.CompilerServices;

// https://devblogs.microsoft.com/dotnet/csharp-15-union-types/
internal interface IUnion
{
    object? Value { get; }
}
#endif
