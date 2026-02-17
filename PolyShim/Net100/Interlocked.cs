#if NET5_0_OR_GREATER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net100_Interlocked
{
    extension(Interlocked)
    {
        public static int And(ref int location1, int value) =>
            Interlocked.And(ref location1, value);

        public static uint And(ref uint location1, uint value)
        {
            ref int loc = ref Unsafe.As<uint, int>(ref location1);
            int result = Interlocked.And(ref loc, (int)value);
            return (uint)result;
        }

        public static long And(ref long location1, long value) =>
            Interlocked.And(ref location1, value);

        public static ulong And(ref ulong location1, ulong value)
        {
            ref long loc = ref Unsafe.As<ulong, long>(ref location1);
            long result = Interlocked.And(ref loc, (long)value);
            return (ulong)result;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and-1(-0@-0)
        public static T And<T>(ref T location1, T value)
            where T : struct
        {
            // Reject floating-point types
            if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
            {
                throw new NotSupportedException(
                    "Only integer primitive types and enums backed by integer types are supported."
                );
            }

            if (typeof(T) == typeof(int))
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = And(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (typeof(T) == typeof(uint))
            {
                ref uint loc = ref Unsafe.As<T, uint>(ref location1);
                uint val = Unsafe.As<T, uint>(ref value);
                uint result = And(ref loc, val);
                return Unsafe.As<uint, T>(ref result);
            }

            if (typeof(T) == typeof(long))
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = And(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            if (typeof(T) == typeof(ulong))
            {
                ref ulong loc = ref Unsafe.As<T, ulong>(ref location1);
                ulong val = Unsafe.As<T, ulong>(ref value);
                ulong result = And(ref loc, val);
                return Unsafe.As<ulong, T>(ref result);
            }

            // For other types (enums), use size-based dispatch
            if (Unsafe.SizeOf<T>() == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = And(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (Unsafe.SizeOf<T>() == 8)
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = And(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            throw new NotSupportedException(
                $"Type {typeof(T).Name} is not supported. Only 4-byte and 8-byte integer types and enums are supported."
            );
        }

        public static int Or(ref int location1, int value) => Interlocked.Or(ref location1, value);

        public static uint Or(ref uint location1, uint value)
        {
            ref int loc = ref Unsafe.As<uint, int>(ref location1);
            int result = Interlocked.Or(ref loc, (int)value);
            return (uint)result;
        }

        public static long Or(ref long location1, long value) =>
            Interlocked.Or(ref location1, value);

        public static ulong Or(ref ulong location1, ulong value)
        {
            ref long loc = ref Unsafe.As<ulong, long>(ref location1);
            long result = Interlocked.Or(ref loc, (long)value);
            return (ulong)result;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.or#system-threading-interlocked-or-1(-0@-0)
        public static T Or<T>(ref T location1, T value)
            where T : struct
        {
            // Reject floating-point types
            if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
            {
                throw new NotSupportedException(
                    "Only integer primitive types and enums backed by integer types are supported."
                );
            }

            if (typeof(T) == typeof(int))
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = Or(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (typeof(T) == typeof(uint))
            {
                ref uint loc = ref Unsafe.As<T, uint>(ref location1);
                uint val = Unsafe.As<T, uint>(ref value);
                uint result = Or(ref loc, val);
                return Unsafe.As<uint, T>(ref result);
            }

            if (typeof(T) == typeof(long))
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = Or(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            if (typeof(T) == typeof(ulong))
            {
                ref ulong loc = ref Unsafe.As<T, ulong>(ref location1);
                ulong val = Unsafe.As<T, ulong>(ref value);
                ulong result = Or(ref loc, val);
                return Unsafe.As<ulong, T>(ref result);
            }

            // For other types (enums), use size-based dispatch
            if (Unsafe.SizeOf<T>() == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = Or(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (Unsafe.SizeOf<T>() == 8)
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = Or(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            throw new NotSupportedException(
                $"Type {typeof(T).Name} is not supported. Only 4-byte and 8-byte integer types and enums are supported."
            );
        }
    }
}
#endif
