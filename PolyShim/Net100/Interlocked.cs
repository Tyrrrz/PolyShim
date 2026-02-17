#if NET5_0_OR_GREATER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net100_Interlocked
{
    extension(System.Threading.Interlocked)
    {
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

            if (Unsafe.SizeOf<T>() == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = System.Threading.Interlocked.And(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (Unsafe.SizeOf<T>() == 8)
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = System.Threading.Interlocked.And(ref loc, val);
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

            if (Unsafe.SizeOf<T>() == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = System.Threading.Interlocked.Or(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            if (Unsafe.SizeOf<T>() == 8)
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = System.Threading.Interlocked.Or(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }

            throw new NotSupportedException(
                $"Type {typeof(T).Name} is not supported. Only 4-byte and 8-byte integer types and enums are supported."
            );
        }
    }
}
#endif
