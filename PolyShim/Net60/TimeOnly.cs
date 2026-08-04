#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.timeonly
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct TimeOnly
    : IComparable,
        IComparable<TimeOnly>,
        IEquatable<TimeOnly>,
        IFormattable
{
    private readonly DateTime _dateTime;

    private TimeOnly(DateTime dateTime) => _dateTime = dateTime;

    public TimeOnly(long ticks)
        : this(
            ticks >= 0 && ticks <= TimeSpan.TicksPerDay - 1
                ? new DateTime(ticks)
                : throw new ArgumentOutOfRangeException(nameof(ticks))
        ) { }

    public TimeOnly(int hour, int minute)
        : this(new TimeSpan(0, hour, minute, 0, 0).Ticks) { }

    public TimeOnly(int hour, int minute, int second)
        : this(new TimeSpan(0, hour, minute, second, 0).Ticks) { }

    public TimeOnly(int hour, int minute, int second, int millisecond)
        : this(new TimeSpan(0, hour, minute, second, millisecond).Ticks) { }

    public static TimeOnly MinValue { get; } = new(0);

    public static TimeOnly MaxValue { get; } = new(TimeSpan.TicksPerDay - 1);

    public int Hour => _dateTime.Hour;

    public int Minute => _dateTime.Minute;

    public int Second => _dateTime.Second;

    public int Millisecond => _dateTime.Millisecond;

    public long Ticks => _dateTime.Ticks;

    private TimeOnly AddTicks(long ticks)
    {
        var newTicks = (_dateTime.Ticks + ticks) % TimeSpan.TicksPerDay;
        if (newTicks < 0)
            newTicks += TimeSpan.TicksPerDay;

        return new TimeOnly(newTicks);
    }

    private TimeOnly AddTicks(long ticks, out int wrappedDays)
    {
        var days = ticks / TimeSpan.TicksPerDay;
        var newTicks = ticks % TimeSpan.TicksPerDay;
        newTicks += _dateTime.Ticks;

        if (newTicks < 0)
        {
            days--;
            newTicks += TimeSpan.TicksPerDay;
        }
        else if (newTicks >= TimeSpan.TicksPerDay)
        {
            days++;
            newTicks -= TimeSpan.TicksPerDay;
        }

        wrappedDays = (int)days;
        return new TimeOnly(newTicks);
    }

    public TimeOnly Add(TimeSpan value) => AddTicks(value.Ticks);

    public TimeOnly Add(TimeSpan value, out int wrappedDays) =>
        AddTicks(value.Ticks, out wrappedDays);

    public TimeOnly AddHours(double value) => Add(TimeSpan.FromHours(value));

    public TimeOnly AddHours(double value, out int wrappedDays) =>
        Add(TimeSpan.FromHours(value), out wrappedDays);

    public TimeOnly AddMinutes(double value) => Add(TimeSpan.FromMinutes(value));

    public TimeOnly AddMinutes(double value, out int wrappedDays) =>
        Add(TimeSpan.FromMinutes(value), out wrappedDays);

    public bool IsBetween(TimeOnly start, TimeOnly end)
    {
        var ticks = _dateTime.Ticks;
        var startTicks = start._dateTime.Ticks;
        var endTicks = end._dateTime.Ticks;

        return startTicks <= endTicks
            ? ticks >= startTicks && ticks < endTicks
            : ticks >= startTicks || ticks < endTicks;
    }

    public static TimeOnly FromDateTime(DateTime dateTime) =>
        new(new DateTime(dateTime.TimeOfDay.Ticks));

    public static TimeOnly FromTimeSpan(TimeSpan timeSpan) => new(new DateTime(timeSpan.Ticks));

    public TimeSpan ToTimeSpan() => new(_dateTime.Ticks);

    public int CompareTo(TimeOnly value) => _dateTime.Ticks.CompareTo(value._dateTime.Ticks);

    public int CompareTo(object? value)
    {
        if (value is null)
            return 1;

        if (value is not TimeOnly timeOnly)
            throw new ArgumentException("Object must be of type TimeOnly.", nameof(value));

        return CompareTo(timeOnly);
    }

    public bool Equals(TimeOnly other) => _dateTime.Ticks == other._dateTime.Ticks;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is TimeOnly other && Equals(other);

    public override int GetHashCode() => _dateTime.Ticks.GetHashCode();

    public override string ToString() => _dateTime.ToString("t", CultureInfo.CurrentCulture);

    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(IFormatProvider? provider) => _dateTime.ToString("t", provider);

    public string ToString(string? format, IFormatProvider? provider)
    {
        if (string.IsNullOrEmpty(format))
            format = "t";

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

    public static TimeOnly Parse(string s) => Parse(s, null, DateTimeStyles.None);

    public static TimeOnly Parse(
        string s,
        IFormatProvider? provider,
        DateTimeStyles style = DateTimeStyles.None
    )
    {
        if (!TryParse(s, provider, style, out var result))
            throw new FormatException($"String '{s}' was not recognized as a valid TimeOnly.");
        return result;
    }

    public static TimeOnly Parse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider = null,
        DateTimeStyles style = DateTimeStyles.None
    ) => Parse(new string(s.ToArray()), provider, style);

    public static TimeOnly ParseExact(string s, string format) =>
        ParseExact(s, format, null, DateTimeStyles.None);

    public static TimeOnly ParseExact(
        string s,
        string format,
        IFormatProvider? provider,
        DateTimeStyles style = DateTimeStyles.None
    )
    {
        if (!TryParseExact(s, format, provider, style, out var result))
            throw new FormatException($"String '{s}' was not recognized as a valid TimeOnly.");
        return result;
    }

    public static TimeOnly ParseExact(string s, string[] formats) =>
        ParseExact(s, formats, null, DateTimeStyles.None);

    public static TimeOnly ParseExact(
        string s,
        string[] formats,
        IFormatProvider? provider,
        DateTimeStyles style = DateTimeStyles.None
    )
    {
        if (!TryParseExact(s, formats, provider, style, out var result))
            throw new FormatException($"String '{s}' was not recognized as a valid TimeOnly.");
        return result;
    }

    public static TimeOnly ParseExact(
        ReadOnlySpan<char> s,
        ReadOnlySpan<char> format,
        IFormatProvider? provider = null,
        DateTimeStyles style = DateTimeStyles.None
    ) => ParseExact(new string(s.ToArray()), new string(format.ToArray()), provider, style);

    public static TimeOnly ParseExact(
        ReadOnlySpan<char> s,
        string[] formats,
        IFormatProvider? provider = null,
        DateTimeStyles style = DateTimeStyles.None
    ) => ParseExact(new string(s.ToArray()), formats, provider, style);

    public static bool TryParse([NotNullWhen(true)] string? s, out TimeOnly result) =>
        TryParse(s, null, DateTimeStyles.None, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        DateTimeStyles style,
        out TimeOnly result
    )
    {
        if (
            s is not null
            && DateTime.TryParse(
                s,
                provider,
                style | DateTimeStyles.NoCurrentDateDefault,
                out var dateTime
            )
            && dateTime.Date == DateTime.MinValue.Date
        )
        {
            result = FromDateTime(dateTime);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> s, out TimeOnly result) =>
        TryParse(new string(s.ToArray()), null, DateTimeStyles.None, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        DateTimeStyles style,
        out TimeOnly result
    ) => TryParse(new string(s.ToArray()), provider, style, out result);

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string? format,
        out TimeOnly result
    ) => TryParseExact(s, format, null, DateTimeStyles.None, out result);

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string? format,
        IFormatProvider? provider,
        DateTimeStyles style,
        out TimeOnly result
    )
    {
        if (
            s is not null
            && format is not null
            && DateTime.TryParseExact(
                s,
                format,
                provider,
                style | DateTimeStyles.NoCurrentDateDefault,
                out var dateTime
            )
            && dateTime.Date == DateTime.MinValue.Date
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
        out TimeOnly result
    ) => TryParseExact(s, formats, null, DateTimeStyles.None, out result);

    public static bool TryParseExact(
        [NotNullWhen(true)] string? s,
        [NotNullWhen(true)] string?[]? formats,
        IFormatProvider? provider,
        DateTimeStyles style,
        out TimeOnly result
    )
    {
        if (
            s is not null
            && formats is not null
            && DateTime.TryParseExact(
                s,
                formats,
                provider,
                style | DateTimeStyles.NoCurrentDateDefault,
                out var dateTime
            )
            && dateTime.Date == DateTime.MinValue.Date
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
        out TimeOnly result
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
        out TimeOnly result
    ) => TryParseExact(new string(s.ToArray()), formats, provider, style, out result);

    public static bool operator ==(TimeOnly left, TimeOnly right) => left.Equals(right);

    public static bool operator !=(TimeOnly left, TimeOnly right) => !left.Equals(right);

    public static bool operator <(TimeOnly left, TimeOnly right) =>
        left._dateTime.Ticks < right._dateTime.Ticks;

    public static bool operator <=(TimeOnly left, TimeOnly right) =>
        left._dateTime.Ticks <= right._dateTime.Ticks;

    public static bool operator >(TimeOnly left, TimeOnly right) =>
        left._dateTime.Ticks > right._dateTime.Ticks;

    public static bool operator >=(TimeOnly left, TimeOnly right) =>
        left._dateTime.Ticks >= right._dateTime.Ticks;

    public static TimeSpan operator -(TimeOnly t1, TimeOnly t2)
    {
        var diff = t1._dateTime.Ticks - t2._dateTime.Ticks;
        if (diff < 0)
            diff += TimeSpan.TicksPerDay;

        return new TimeSpan(diff);
    }
}
#endif
