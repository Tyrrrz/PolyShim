#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.standardformat
[ExcludeFromCodeCoverage]
internal readonly struct StandardFormat : IEquatable<StandardFormat>
{
    private readonly byte _format;
    private readonly byte _precision;

    public StandardFormat(char symbol, byte precision = default)
    {
        _format = (byte)symbol;
        _precision = precision;
    }

    public char Symbol => (char)_format;

    public byte Precision => _precision;

    public bool IsDefault => _format == 0 && _precision == 0;

    public static StandardFormat Parse(ReadOnlySpan<char> format)
    {
        if (format.Length == 0)
            return default;

        var symbol = format[0];
        byte precision = 0;

        if (format.Length > 1)
        {
            var precisionStr = new string(format.Slice(1).ToArray());
            if (!byte.TryParse(precisionStr, out precision))
                throw new FormatException("Invalid format.");
        }

        return new StandardFormat(symbol, precision);
    }

    public static StandardFormat Parse(string format) => Parse(format.AsSpan());

    public override bool Equals(object? obj) => obj is StandardFormat other && Equals(other);

    public bool Equals(StandardFormat other) =>
        _format == other._format && _precision == other._precision;

    public override int GetHashCode() => _format.GetHashCode() ^ _precision.GetHashCode();

    public static bool operator ==(StandardFormat left, StandardFormat right) => left.Equals(right);

    public static bool operator !=(StandardFormat left, StandardFormat right) =>
        !left.Equals(right);

    public override string ToString()
    {
        if (IsDefault)
            return string.Empty;

        if (_precision == 0)
            return Symbol.ToString();

        return Symbol + _precision.ToString();
    }
}
#endif
