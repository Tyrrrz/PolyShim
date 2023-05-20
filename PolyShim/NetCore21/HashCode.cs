﻿#if !FEATURE_HASHCODE
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/en-us/dotnet/api/system.hashcode
[ExcludeFromCodeCoverage]
internal partial class HashCode
{
    private int _hash;

    private void Add(int hashCode) =>
        _hash = _hash * 31 + hashCode;

    public void Add<T>(T value)
    {
        if (value is not null)
            Add(value.GetHashCode());
    }

    public void Add<T>(T value, IEqualityComparer<T> comparer)
    {
        if (value is not null)
            Add(comparer.GetHashCode(value));
    }

    public int ToHashCode() => _hash;
}

internal partial class HashCode
{
    public static int Combine<T1>(T1 value1)
    {
        var hc = new HashCode();
        hc.Add(value1);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2>(T1 value1, T2 value2)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5,
        T6 value6)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        hc.Add(value6);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5,
        T6 value6, T7 value7)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        hc.Add(value6);
        hc.Add(value7);
        return hc.ToHashCode();
    }

    public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5,
        T6 value6, T7 value7, T8 value8)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        hc.Add(value6);
        hc.Add(value7);
        hc.Add(value8);
        return hc.ToHashCode();
    }
}
#endif