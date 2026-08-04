#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.dateonly
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct DateOnly
    : IComparable,
        IComparable<DateOnly>,
        IEquatable<DateOnly>,
        IFormattable
{
    private readonly DateTime _dateTime;

    private DateOnly(DateTime dateTime) =>
        _dateTime = DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Unspecified);

    public DateOnly(int year, int month, int day)
        : this(new DateTime(year, month, day)) { }

    public static DateOnly MinValue { get; } = new(DateTime.MinValue);

    public static DateOnly MaxValue { get; } = new(DateTime.MaxValue.Date);

    public int Year => _dateTime.Year;

    public int Month => _dateTime.Month;

    public int Day => _dateTime.Day;

    public DayOfWeek DayOfWeek => _dateTime.DayOfWeek;

    public int DayOfYear => _dateTime.DayOfYear;

    public int DayNumber => (int)(_dateTime.Ticks / TimeSpan.TicksPerDay);

    public DateOnly AddDays(int value) => new(_dateTime.AddDays(value));

    public DateOnly AddMonths(int value) => new(_dateTime.AddMonths(value));

    public DateOnly AddYears(int value) => new(_dateTime.AddYears(value));

    public static DateOnly FromDateTime(DateTime dateTime) => new(dateTime);

    public static DateOnly FromDayNumber(int dayNumber)
    {
        if (dayNumber < 0 || dayNumber > MaxValue.DayNumber)
            throw new ArgumentOutOfRangeException(nameof(dayNumber));

        return new DateOnly(DateTime.MinValue.AddDays(dayNumber));
    }

    public DateTime ToDateTime(TimeOnly time) => _dateTime.Add(time.ToTimeSpan());

    public DateTime ToDateTime(TimeOnly time, DateTimeKind kind) =>
        DateTime.SpecifyKind(ToDateTime(time), kind);

    public int CompareTo(DateOnly value) => _dateTime.CompareTo(value._dateTime);

    public int CompareTo(object? value)
    {
        if (value is null)
            return 1;

        if (value is not DateOnly dateOnly)
            throw new ArgumentException("Object must be of type DateOnly.", nameof(value));

        return CompareTo(dateOnly);
    }

    public bool Equals(DateOnly other) => _dateTime == other._dateTime;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DateOnly other && Equals(other);

    public override int GetHashCode() => _dateTime.GetHashCode();

    public override string ToString() => _dateTime.ToString("d", CultureInfo.CurrentCulture);

    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(IFormatProvider? provider) => _dateTime.ToString("d", provider);

    public string ToString(string? format, IFormatProvider? provider)
    {
        if (string.IsNullOrEmpty(format))
            format = "d";

        return _dateTime.ToString(format, provider);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    )
    {
        var formatted = ToString(format.IsEmpty ? null : new string(format.ToArray()), provider);
        if (formatted.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        formatted.AsSpan().CopyTo(destination);
        charsWritten = formatted.Length;
        return true;
    }

    public static DateOnly Parse(string s) => Parse(s, null, DateTimeStyles.None);

    public static DateOnly Parse(
        string s,
        IFormatProvider? provider,
        DateTimeStyles style = DateTimeStyles.None
    )
    {
        if (!TryParse(s, provider, style, out var result))
            throw new FormatException($"String '{s}' was not recognized as a valid DateOnly.");
        return result;
    }

    public static DateOnly Parse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider = null,
        DateTimeStyles style = DateTimeStyles.None
    ) => Parse(new string(s.ToArray()), provider, style);

    public static DateOnly ParseExact(string s, string format) =>
        ParseExact(s, format, null, DateTimeStyles.None);

    public static DateOnly ParseExact(
        string s,
        string format,
        IFormatProvider? provider,
        DateTimeStyles style = DateTimeStyles.None
    )
    {
        if (!TryParseExact(s, format, provider, style, out var result))
            throw new FormatException($"String '{s}' was not recognized as a valid DateOnly.");
        return result;
    }

    public static DateOnly ParseExact(string s, string[] formats) =>
        ParseExact(s, formats, null, DateTimeStyles.None);

    public static DateOnly ParseExact(
        string s,
        string[] formats,
        IFormatProvider? provider,
        DateTimeStyles style = DateTimeStyles.None
    )
    {
        if (!TryParseExact(s, formats, provider, style, out var result))
            throw new FormatException($"String '{s}' was not recognized as a valid DateOnly.");
        return result;
    }

    public static DateOnly ParseExact(
        ReadOnlySpan<char> s,
        ReadOnlySpan<char> format,
        IFormatProvider? provider = null,
        DateTimeStyles style = DateTimeStyles.None
    ) => ParseExact(new string(s.ToArray()), new string(format.ToArray()), provider, style);

    public static DateOnly ParseExact(
        ReadOnlySpan<char> s,
        string[] formats,
        IFormatProvider? provider = null,
        DateTimeStyles style = DateTimeStyles.None
    ) => ParseExact(new string(s.ToArray()), formats, provider, style);

    public static bool TryParse([NotNullWhen(true)] string? s, out DateOnly result) =>
        TryParse(s, null, DateTimeStyles.None, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        DateTimeStyles style,
        out DateOnly result
    )
    {
        if (
            s is not null
            && DateTime.TryParse(s, provider, style, out var dateTime)
            && dateTime.TimeOfDay == TimeSpan.Zero
        )
        {
            result = FromDateTime(dateTime);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        DateTimeStyles style,
        out DateOnly result
    ) => TryParse(new string(s.ToArray()), provider, style, out result);

    public static bool TryParse(ReadOnlySpan<char> s, out DateOnly result) =>
        TryParse(new string(s.ToArray()), null, DateTimeStyles.None, out result);

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string? format,
        out DateOnly result
    ) => TryParseExact(s, format, null, DateTimeStyles.None, out result);

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string? format,
        IFormatProvider? provider,
        DateTimeStyles style,
        out DateOnly result
    )
    {
        if (
            s is not null
            && format is not null
            && DateTime.TryParseExact(s, format, provider, style, out var dateTime)
            && dateTime.TimeOfDay == TimeSpan.Zero
        )
        {
            result = FromDateTime(dateTime);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string?[]? formats,
        out DateOnly result
    ) => TryParseExact(s, formats, null, DateTimeStyles.None, out result);

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string?[]? formats,
        IFormatProvider? provider,
        DateTimeStyles style,
        out DateOnly result
    )
    {
        if (
            s is not null
            && formats is not null
            && DateTime.TryParseExact(s, formats, provider, style, out var dateTime)
            && dateTime.TimeOfDay == TimeSpan.Zero
        )
        {
            result = FromDateTime(dateTime);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParseExact(
        ReadOnlySpan<char> s,
        ReadOnlySpan<char> format,
        IFormatProvider? provider,
        DateTimeStyles style,
        out DateOnly result
    ) =>
        TryParseExact(
            new string(s.ToArray()),
            new string(format.ToArray()),
            provider,
            style,
            out result
        );

    public static bool TryParseExact(
        ReadOnlySpan<char> s,
        string?[]? formats,
        IFormatProvider? provider,
        DateTimeStyles style,
        out DateOnly result
    ) => TryParseExact(new string(s.ToArray()), formats, provider, style, out result);

    public static bool operator ==(DateOnly left, DateOnly right) => left.Equals(right);

    public static bool operator !=(DateOnly left, DateOnly right) => !left.Equals(right);

    public static bool operator <(DateOnly left, DateOnly right) =>
        left._dateTime < right._dateTime;

    public static bool operator <=(DateOnly left, DateOnly right) =>
        left._dateTime <= right._dateTime;

    public static bool operator >(DateOnly left, DateOnly right) =>
        left._dateTime > right._dateTime;

    public static bool operator >=(DateOnly left, DateOnly right) =>
        left._dateTime >= right._dateTime;
}
#endif
