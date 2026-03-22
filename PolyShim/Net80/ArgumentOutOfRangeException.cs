#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_ArgumentOutOfRangeException
{
    extension(ArgumentOutOfRangeException)
    {
        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnegative
        public static void ThrowIfNegative<T>(
            T value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default!) < 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be a non-negative value."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnegativeorzero
        public static void ThrowIfNegativeOrZero<T>(
            T value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default!) <= 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be a positive value."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifzero
        public static void ThrowIfZero<T>(
            T value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default!) == 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be a non-zero value."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifgreaterthan
        public static void ThrowIfGreaterThan<T>(
            T value,
            T other,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(value, other) > 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be less than or equal to '{other}'."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifgreaterthanorequal
        public static void ThrowIfGreaterThanOrEqual<T>(
            T value,
            T other,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(value, other) >= 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be less than '{other}'."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwiflessthan
        public static void ThrowIfLessThan<T>(
            T value,
            T other,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(value, other) < 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be greater than or equal to '{other}'."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwiflessthanorequal
        public static void ThrowIfLessThanOrEqual<T>(
            T value,
            T other,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(value, other) <= 0)
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be greater than '{other}'."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifequal
        public static void ThrowIfEqual<T>(
            T value,
            T other,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : IEquatable<T>
        {
            if (EqualityComparer<T>.Default.Equals(value, other))
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must not be equal to '{other}'."
                );
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnotequal
        public static void ThrowIfNotEqual<T>(
            T value,
            T other,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : IEquatable<T>
        {
            if (!EqualityComparer<T>.Default.Equals(value, other))
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be equal to '{other}'."
                );
        }
    }
}
#endif
