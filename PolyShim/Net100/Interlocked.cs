#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
#if NET5_0_OR_GREATER
using Unsafe = System.Runtime.CompilerServices.Unsafe;
#endif

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net100_Interlocked
{
    extension(Interlocked)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and-1(-0@-0)
#if NET5_0_OR_GREATER
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

            // Only handle known types - requires Unsafe for generic type handling
            if (typeof(T) == typeof(int))
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = Interlocked.And(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (typeof(T) == typeof(uint))
            {
                ref uint loc = ref Unsafe.As<T, uint>(ref location1);
                uint val = Unsafe.As<T, uint>(ref value);
                uint result = Interlocked.And(ref loc, val);
                return Unsafe.As<uint, T>(ref result);
            }

            if (typeof(T) == typeof(long))
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = Interlocked.And(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            if (typeof(T) == typeof(ulong))
            {
                ref ulong loc = ref Unsafe.As<T, ulong>(ref location1);
                ulong val = Unsafe.As<T, ulong>(ref value);
                ulong result = Interlocked.And(ref loc, val);
                return Unsafe.As<ulong, T>(ref result);
            }

            // For other types (enums), dispatch based on size
            var size = Unsafe.SizeOf<T>();
            if (size == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = Interlocked.And(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (size == 8)
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = Interlocked.And(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            throw new NotSupportedException(
                $"Type {typeof(T).Name} is not supported. Only 4-byte and 8-byte integer types and enums are supported."
            );
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

            // Only handle known types - requires Unsafe for generic type handling
            if (typeof(T) == typeof(int))
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = Interlocked.Or(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (typeof(T) == typeof(uint))
            {
                ref uint loc = ref Unsafe.As<T, uint>(ref location1);
                uint val = Unsafe.As<T, uint>(ref value);
                uint result = Interlocked.Or(ref loc, val);
                return Unsafe.As<uint, T>(ref result);
            }

            if (typeof(T) == typeof(long))
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = Interlocked.Or(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            if (typeof(T) == typeof(ulong))
            {
                ref ulong loc = ref Unsafe.As<T, ulong>(ref location1);
                ulong val = Unsafe.As<T, ulong>(ref value);
                ulong result = Interlocked.Or(ref loc, val);
                return Unsafe.As<ulong, T>(ref result);
            }

            // For other types (enums), dispatch based on size
            var size = Unsafe.SizeOf<T>();
            if (size == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = Interlocked.Or(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (size == 8)
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = Interlocked.Or(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            throw new NotSupportedException(
                $"Type {typeof(T).Name} is not supported. Only 4-byte and 8-byte integer types and enums are supported."
            );
        }
#endif
    }
}
#endif
