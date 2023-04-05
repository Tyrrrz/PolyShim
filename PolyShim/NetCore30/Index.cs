#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/en-us/dotnet/api/system.index
[ExcludeFromCodeCoverage]
internal readonly struct Index : IEquatable<Index>
{
    private readonly int _value;

    public Index(int value, bool fromEnd = false)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");

        _value = fromEnd ? ~value : value;
    }

    private Index(int value) => _value = value;

    public static Index Start => new(0);

    public static Index End => new(~0);

    public static Index FromStart(int value) => value >= 0
        ? new Index(value)
        : throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");

    public static Index FromEnd(int value) => value >= 0
        ? new Index(~value)
        : throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");

    public int Value => _value < 0 ? ~_value : _value;

    public bool IsFromEnd => _value < 0;

    public int GetOffset(int length)
    {
        var offset = _value;
        if (IsFromEnd)
            offset += length + 1;

        return offset;
    }

    public override bool Equals(object? value) => value is Index index && _value == index._value;

    public bool Equals(Index other) => _value == other._value;

    public override int GetHashCode() => _value;

    public static implicit operator Index(int value) => FromStart(value);

    public override string ToString()
    {
        if (IsFromEnd)
            return "^" + (uint)Value;

        return ((uint)Value).ToString();
    }
}
#endif