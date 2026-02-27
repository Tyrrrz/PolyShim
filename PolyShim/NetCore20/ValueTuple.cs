#if !FEATURE_VALUETUPLE
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.valuetuple
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple : IEquatable<ValueTuple>, IComparable<ValueTuple>, IComparable
{
    public override bool Equals(object? obj) => obj is ValueTuple;

    public bool Equals(ValueTuple other) => true;

    public int CompareTo(ValueTuple other) => 0;

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple => 0,
            _ => throw new ArgumentException("Object must be of type ValueTuple"),
        };

    public override int GetHashCode() => 0;

    public override string ToString() => "()";

    public static bool operator ==(ValueTuple left, ValueTuple right) => true;

    public static bool operator !=(ValueTuple left, ValueTuple right) => false;

    public static bool operator <(ValueTuple left, ValueTuple right) => false;

    public static bool operator >(ValueTuple left, ValueTuple right) => false;

    public static bool operator <=(ValueTuple left, ValueTuple right) => true;

    public static bool operator >=(ValueTuple left, ValueTuple right) => true;

    public static ValueTuple<T1> Create<T1>(T1 item1) => new(item1);

    public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new(item1, item2);

    public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) =>
        new(item1, item2, item3);

    public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(
        T1 item1,
        T2 item2,
        T3 item3,
        T4 item4
    ) => new(item1, item2, item3, item4);

    public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(
        T1 item1,
        T2 item2,
        T3 item3,
        T4 item4,
        T5 item5
    ) => new(item1, item2, item3, item4, item5);

    public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(
        T1 item1,
        T2 item2,
        T3 item3,
        T4 item4,
        T5 item5,
        T6 item6
    ) => new(item1, item2, item3, item4, item5, item6);

    public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(
        T1 item1,
        T2 item2,
        T3 item3,
        T4 item4,
        T5 item5,
        T6 item6,
        T7 item7
    ) => new(item1, item2, item3, item4, item5, item6, item7);

    public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(
        T1 item1,
        T2 item2,
        T3 item3,
        T4 item4,
        T5 item5,
        T6 item6,
        T7 item7,
        T8 item8
    ) => new(item1, item2, item3, item4, item5, item6, item7, item8);
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1>(T1 item1)
    : IEquatable<ValueTuple<T1>>,
        IComparable<ValueTuple<T1>>,
        IComparable
{
    public readonly T1 Item1 = item1;

    public override bool Equals(object? obj) => obj is ValueTuple<T1> other && Equals(other);

    public bool Equals(ValueTuple<T1> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1);

    public int CompareTo(ValueTuple<T1> other) => Comparer<T1>.Default.Compare(Item1, other.Item1);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1> other => CompareTo(other),
            _ => throw new ArgumentException("Object must be of type ValueTuple<T1>"),
        };

    public override int GetHashCode() => HashCode.Combine(Item1);

    public override string ToString() => $"({Item1})";

    public static bool operator ==(ValueTuple<T1> left, ValueTuple<T1> right) => left.Equals(right);

    public static bool operator !=(ValueTuple<T1> left, ValueTuple<T1> right) =>
        !left.Equals(right);

    public static bool operator <(ValueTuple<T1> left, ValueTuple<T1> right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(ValueTuple<T1> left, ValueTuple<T1> right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(ValueTuple<T1> left, ValueTuple<T1> right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >=(ValueTuple<T1> left, ValueTuple<T1> right) =>
        left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-2
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2>(T1 item1, T2 item2)
    : IEquatable<ValueTuple<T1, T2>>,
        IComparable<ValueTuple<T1, T2>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;

    public override bool Equals(object? obj) => obj is ValueTuple<T1, T2> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2);

    public int CompareTo(ValueTuple<T1, T2> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
            : Comparer<T2>.Default.Compare(Item2, other.Item2);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2> other => CompareTo(other),
            _ => throw new ArgumentException("Object must be of type ValueTuple<T1, T2>"),
        };

    public override int GetHashCode() => HashCode.Combine(Item1, Item2);

    public override string ToString() => $"({Item1}, {Item2})";

    public static bool operator ==(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) =>
        left.Equals(right);

    public static bool operator !=(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) =>
        !left.Equals(right);

    public static bool operator <(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >=(ValueTuple<T1, T2> left, ValueTuple<T1, T2> right) =>
        left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-3
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
    : IEquatable<ValueTuple<T1, T2, T3>>,
        IComparable<ValueTuple<T1, T2, T3>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;
    public readonly T3 Item3 = item3;

    public override bool Equals(object? obj) =>
        obj is ValueTuple<T1, T2, T3> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2, T3> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
        && EqualityComparer<T3>.Default.Equals(Item3, other.Item3);

    public int CompareTo(ValueTuple<T1, T2, T3> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
        : Comparer<T2>.Default.Compare(Item2, other.Item2) != 0
            ? Comparer<T2>.Default.Compare(Item2, other.Item2)
        : Comparer<T3>.Default.Compare(Item3, other.Item3);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2, T3> other => CompareTo(other),
            _ => throw new ArgumentException("Object must be of type ValueTuple<T1, T2, T3>"),
        };

    public override int GetHashCode() => HashCode.Combine(Item1, Item2, Item3);

    public override string ToString() => $"({Item1}, {Item2}, {Item3})";

    public static bool operator ==(ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right) =>
        left.Equals(right);

    public static bool operator !=(ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right) =>
        !left.Equals(right);

    public static bool operator <(ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >=(ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right) =>
        left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-4
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
    : IEquatable<ValueTuple<T1, T2, T3, T4>>,
        IComparable<ValueTuple<T1, T2, T3, T4>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;
    public readonly T3 Item3 = item3;
    public readonly T4 Item4 = item4;

    public override bool Equals(object? obj) =>
        obj is ValueTuple<T1, T2, T3, T4> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2, T3, T4> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
        && EqualityComparer<T3>.Default.Equals(Item3, other.Item3)
        && EqualityComparer<T4>.Default.Equals(Item4, other.Item4);

    public int CompareTo(ValueTuple<T1, T2, T3, T4> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
        : Comparer<T2>.Default.Compare(Item2, other.Item2) != 0
            ? Comparer<T2>.Default.Compare(Item2, other.Item2)
        : Comparer<T3>.Default.Compare(Item3, other.Item3) != 0
            ? Comparer<T3>.Default.Compare(Item3, other.Item3)
        : Comparer<T4>.Default.Compare(Item4, other.Item4);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2, T3, T4> other => CompareTo(other),
            _ => throw new ArgumentException("Object must be of type ValueTuple<T1, T2, T3, T4>"),
        };

    public override int GetHashCode() => HashCode.Combine(Item1, Item2, Item3, Item4);

    public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4})";

    public static bool operator ==(
        ValueTuple<T1, T2, T3, T4> left,
        ValueTuple<T1, T2, T3, T4> right
    ) => left.Equals(right);

    public static bool operator !=(
        ValueTuple<T1, T2, T3, T4> left,
        ValueTuple<T1, T2, T3, T4> right
    ) => !left.Equals(right);

    public static bool operator <(
        ValueTuple<T1, T2, T3, T4> left,
        ValueTuple<T1, T2, T3, T4> right
    ) => left.CompareTo(right) < 0;

    public static bool operator >(
        ValueTuple<T1, T2, T3, T4> left,
        ValueTuple<T1, T2, T3, T4> right
    ) => left.CompareTo(right) > 0;

    public static bool operator <=(
        ValueTuple<T1, T2, T3, T4> left,
        ValueTuple<T1, T2, T3, T4> right
    ) => left.CompareTo(right) <= 0;

    public static bool operator >=(
        ValueTuple<T1, T2, T3, T4> left,
        ValueTuple<T1, T2, T3, T4> right
    ) => left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-5
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2, T3, T4, T5>(
    T1 item1,
    T2 item2,
    T3 item3,
    T4 item4,
    T5 item5
)
    : IEquatable<ValueTuple<T1, T2, T3, T4, T5>>,
        IComparable<ValueTuple<T1, T2, T3, T4, T5>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;
    public readonly T3 Item3 = item3;
    public readonly T4 Item4 = item4;
    public readonly T5 Item5 = item5;

    public override bool Equals(object? obj) =>
        obj is ValueTuple<T1, T2, T3, T4, T5> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
        && EqualityComparer<T3>.Default.Equals(Item3, other.Item3)
        && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
        && EqualityComparer<T5>.Default.Equals(Item5, other.Item5);

    public int CompareTo(ValueTuple<T1, T2, T3, T4, T5> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
        : Comparer<T2>.Default.Compare(Item2, other.Item2) != 0
            ? Comparer<T2>.Default.Compare(Item2, other.Item2)
        : Comparer<T3>.Default.Compare(Item3, other.Item3) != 0
            ? Comparer<T3>.Default.Compare(Item3, other.Item3)
        : Comparer<T4>.Default.Compare(Item4, other.Item4) != 0
            ? Comparer<T4>.Default.Compare(Item4, other.Item4)
        : Comparer<T5>.Default.Compare(Item5, other.Item5);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2, T3, T4, T5> other => CompareTo(other),
            _ => throw new ArgumentException(
                "Object must be of type ValueTuple<T1, T2, T3, T4, T5>"
            ),
        };

    public override int GetHashCode() => HashCode.Combine(Item1, Item2, Item3, Item4, Item5);

    public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5})";

    public static bool operator ==(
        ValueTuple<T1, T2, T3, T4, T5> left,
        ValueTuple<T1, T2, T3, T4, T5> right
    ) => left.Equals(right);

    public static bool operator !=(
        ValueTuple<T1, T2, T3, T4, T5> left,
        ValueTuple<T1, T2, T3, T4, T5> right
    ) => !left.Equals(right);

    public static bool operator <(
        ValueTuple<T1, T2, T3, T4, T5> left,
        ValueTuple<T1, T2, T3, T4, T5> right
    ) => left.CompareTo(right) < 0;

    public static bool operator >(
        ValueTuple<T1, T2, T3, T4, T5> left,
        ValueTuple<T1, T2, T3, T4, T5> right
    ) => left.CompareTo(right) > 0;

    public static bool operator <=(
        ValueTuple<T1, T2, T3, T4, T5> left,
        ValueTuple<T1, T2, T3, T4, T5> right
    ) => left.CompareTo(right) <= 0;

    public static bool operator >=(
        ValueTuple<T1, T2, T3, T4, T5> left,
        ValueTuple<T1, T2, T3, T4, T5> right
    ) => left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-6
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2, T3, T4, T5, T6>(
    T1 item1,
    T2 item2,
    T3 item3,
    T4 item4,
    T5 item5,
    T6 item6
)
    : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6>>,
        IComparable<ValueTuple<T1, T2, T3, T4, T5, T6>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;
    public readonly T3 Item3 = item3;
    public readonly T4 Item4 = item4;
    public readonly T5 Item5 = item5;
    public readonly T6 Item6 = item6;

    public override bool Equals(object? obj) =>
        obj is ValueTuple<T1, T2, T3, T4, T5, T6> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
        && EqualityComparer<T3>.Default.Equals(Item3, other.Item3)
        && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
        && EqualityComparer<T5>.Default.Equals(Item5, other.Item5)
        && EqualityComparer<T6>.Default.Equals(Item6, other.Item6);

    public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
        : Comparer<T2>.Default.Compare(Item2, other.Item2) != 0
            ? Comparer<T2>.Default.Compare(Item2, other.Item2)
        : Comparer<T3>.Default.Compare(Item3, other.Item3) != 0
            ? Comparer<T3>.Default.Compare(Item3, other.Item3)
        : Comparer<T4>.Default.Compare(Item4, other.Item4) != 0
            ? Comparer<T4>.Default.Compare(Item4, other.Item4)
        : Comparer<T5>.Default.Compare(Item5, other.Item5) != 0
            ? Comparer<T5>.Default.Compare(Item5, other.Item5)
        : Comparer<T6>.Default.Compare(Item6, other.Item6);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2, T3, T4, T5, T6> other => CompareTo(other),
            _ => throw new ArgumentException(
                "Object must be of type ValueTuple<T1, T2, T3, T4, T5, T6>"
            ),
        };

    public override int GetHashCode() => HashCode.Combine(Item1, Item2, Item3, Item4, Item5, Item6);

    public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";

    public static bool operator ==(
        ValueTuple<T1, T2, T3, T4, T5, T6> left,
        ValueTuple<T1, T2, T3, T4, T5, T6> right
    ) => left.Equals(right);

    public static bool operator !=(
        ValueTuple<T1, T2, T3, T4, T5, T6> left,
        ValueTuple<T1, T2, T3, T4, T5, T6> right
    ) => !left.Equals(right);

    public static bool operator <(
        ValueTuple<T1, T2, T3, T4, T5, T6> left,
        ValueTuple<T1, T2, T3, T4, T5, T6> right
    ) => left.CompareTo(right) < 0;

    public static bool operator >(
        ValueTuple<T1, T2, T3, T4, T5, T6> left,
        ValueTuple<T1, T2, T3, T4, T5, T6> right
    ) => left.CompareTo(right) > 0;

    public static bool operator <=(
        ValueTuple<T1, T2, T3, T4, T5, T6> left,
        ValueTuple<T1, T2, T3, T4, T5, T6> right
    ) => left.CompareTo(right) <= 0;

    public static bool operator >=(
        ValueTuple<T1, T2, T3, T4, T5, T6> left,
        ValueTuple<T1, T2, T3, T4, T5, T6> right
    ) => left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-7
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2, T3, T4, T5, T6, T7>(
    T1 item1,
    T2 item2,
    T3 item3,
    T4 item4,
    T5 item5,
    T6 item6,
    T7 item7
)
    : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>,
        IComparable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;
    public readonly T3 Item3 = item3;
    public readonly T4 Item4 = item4;
    public readonly T5 Item5 = item5;
    public readonly T6 Item6 = item6;
    public readonly T7 Item7 = item7;

    public override bool Equals(object? obj) =>
        obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
        && EqualityComparer<T3>.Default.Equals(Item3, other.Item3)
        && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
        && EqualityComparer<T5>.Default.Equals(Item5, other.Item5)
        && EqualityComparer<T6>.Default.Equals(Item6, other.Item6)
        && EqualityComparer<T7>.Default.Equals(Item7, other.Item7);

    public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6, T7> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
        : Comparer<T2>.Default.Compare(Item2, other.Item2) != 0
            ? Comparer<T2>.Default.Compare(Item2, other.Item2)
        : Comparer<T3>.Default.Compare(Item3, other.Item3) != 0
            ? Comparer<T3>.Default.Compare(Item3, other.Item3)
        : Comparer<T4>.Default.Compare(Item4, other.Item4) != 0
            ? Comparer<T4>.Default.Compare(Item4, other.Item4)
        : Comparer<T5>.Default.Compare(Item5, other.Item5) != 0
            ? Comparer<T5>.Default.Compare(Item5, other.Item5)
        : Comparer<T6>.Default.Compare(Item6, other.Item6) != 0
            ? Comparer<T6>.Default.Compare(Item6, other.Item6)
        : Comparer<T7>.Default.Compare(Item7, other.Item7);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2, T3, T4, T5, T6, T7> other => CompareTo(other),
            _ => throw new ArgumentException(
                "Object must be of type ValueTuple<T1, T2, T3, T4, T5, T6, T7>"
            ),
        };

    public override int GetHashCode() =>
        HashCode.Combine(Item1, Item2, Item3, Item4, Item5, Item6, Item7);

    public override string ToString() =>
        $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";

    public static bool operator ==(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> right
    ) => left.Equals(right);

    public static bool operator !=(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> right
    ) => !left.Equals(right);

    public static bool operator <(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> right
    ) => left.CompareTo(right) < 0;

    public static bool operator >(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> right
    ) => left.CompareTo(right) > 0;

    public static bool operator <=(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> right
    ) => left.CompareTo(right) <= 0;

    public static bool operator >=(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> right
    ) => left.CompareTo(right) >= 0;
}

// https://learn.microsoft.com/dotnet/api/system.valuetuple-8
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
    T1 item1,
    T2 item2,
    T3 item3,
    T4 item4,
    T5 item5,
    T6 item6,
    T7 item7,
    TRest rest
)
    : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>,
        IComparable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>,
        IComparable
{
    public readonly T1 Item1 = item1;
    public readonly T2 Item2 = item2;
    public readonly T3 Item3 = item3;
    public readonly T4 Item4 = item4;
    public readonly T5 Item5 = item5;
    public readonly T6 Item6 = item6;
    public readonly T7 Item7 = item7;
    public readonly TRest Rest = rest;

    public override bool Equals(object? obj) =>
        obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other && Equals(other);

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other) =>
        EqualityComparer<T1>.Default.Equals(Item1, other.Item1)
        && EqualityComparer<T2>.Default.Equals(Item2, other.Item2)
        && EqualityComparer<T3>.Default.Equals(Item3, other.Item3)
        && EqualityComparer<T4>.Default.Equals(Item4, other.Item4)
        && EqualityComparer<T5>.Default.Equals(Item5, other.Item5)
        && EqualityComparer<T6>.Default.Equals(Item6, other.Item6)
        && EqualityComparer<T7>.Default.Equals(Item7, other.Item7)
        && EqualityComparer<TRest>.Default.Equals(Rest, other.Rest);

    public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other) =>
        Comparer<T1>.Default.Compare(Item1, other.Item1) != 0
            ? Comparer<T1>.Default.Compare(Item1, other.Item1)
        : Comparer<T2>.Default.Compare(Item2, other.Item2) != 0
            ? Comparer<T2>.Default.Compare(Item2, other.Item2)
        : Comparer<T3>.Default.Compare(Item3, other.Item3) != 0
            ? Comparer<T3>.Default.Compare(Item3, other.Item3)
        : Comparer<T4>.Default.Compare(Item4, other.Item4) != 0
            ? Comparer<T4>.Default.Compare(Item4, other.Item4)
        : Comparer<T5>.Default.Compare(Item5, other.Item5) != 0
            ? Comparer<T5>.Default.Compare(Item5, other.Item5)
        : Comparer<T6>.Default.Compare(Item6, other.Item6) != 0
            ? Comparer<T6>.Default.Compare(Item6, other.Item6)
        : Comparer<T7>.Default.Compare(Item7, other.Item7) != 0
            ? Comparer<T7>.Default.Compare(Item7, other.Item7)
        : Comparer<TRest>.Default.Compare(Rest, other.Rest);

    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other => CompareTo(other),
            _ => throw new ArgumentException(
                "Object must be of type ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>"
            ),
        };

    public override int GetHashCode() =>
        HashCode.Combine(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Rest);

    public override string ToString() =>
        $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {Rest})";

    public static bool operator ==(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> right
    ) => left.Equals(right);

    public static bool operator !=(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> right
    ) => !left.Equals(right);

    public static bool operator <(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> right
    ) => left.CompareTo(right) < 0;

    public static bool operator >(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> right
    ) => left.CompareTo(right) > 0;

    public static bool operator <=(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> right
    ) => left.CompareTo(right) <= 0;

    public static bool operator >=(
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> left,
        ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> right
    ) => left.CompareTo(right) >= 0;
}
#endif
