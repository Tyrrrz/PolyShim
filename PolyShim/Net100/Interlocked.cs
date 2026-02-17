#if (NETCOREAPP) || (NETFRAMEWORK) || (NETSTANDARD)
#if NET9_0_OR_GREATER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net100_Interlocked
{
    extension(global::System.Threading.Interlocked)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and-1(-0@-0)
        public static T And<T>(ref T location1, T value)
            where T : struct
        {
            // Only integer primitive types and enum types backed by integer types are supported.
            // Floating-point types and floating-point backed enums are not supported.
            if (
                (!typeof(T).IsPrimitive && !typeof(T).IsEnum)
                || typeof(T) == typeof(float)
                || typeof(T) == typeof(double)
                || (
                    typeof(T).IsEnum
                    && (
                        typeof(T).GetEnumUnderlyingType() == typeof(float)
                        || typeof(T).GetEnumUnderlyingType() == typeof(double)
                    )
                )
            )
            {
                throw new NotSupportedException(
                    "Only integer primitive types and enums backed by integer types are supported."
                );
            }

            // For all types, use CompareExchange-based implementation since we're polyfilling
            // the generic overload that doesn't exist yet
            if (Unsafe.SizeOf<T>() == 1)
            {
                ref byte loc = ref Unsafe.As<T, byte>(ref location1);
                byte val = Unsafe.As<T, byte>(ref value);
                byte current = loc;
                while (true)
                {
                    byte newValue = (byte)(current & val);
                    byte oldValue = global::System.Threading.Interlocked.CompareExchange<byte>(
                        ref loc,
                        newValue,
                        current
                    );
                    if (oldValue == current)
                    {
                        byte result = oldValue;
                        return Unsafe.As<byte, T>(ref result);
                    }
                    current = oldValue;
                }
            }

            if (Unsafe.SizeOf<T>() == 2)
            {
                ref ushort loc = ref Unsafe.As<T, ushort>(ref location1);
                ushort val = Unsafe.As<T, ushort>(ref value);
                ushort current = loc;
                while (true)
                {
                    ushort newValue = (ushort)(current & val);
                    ushort oldValue = global::System.Threading.Interlocked.CompareExchange<ushort>(
                        ref loc,
                        newValue,
                        current
                    );
                    if (oldValue == current)
                    {
                        ushort result = oldValue;
                        return Unsafe.As<ushort, T>(ref result);
                    }
                    current = oldValue;
                }
            }

            if (Unsafe.SizeOf<T>() == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = global::System.Threading.Interlocked.And(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            // Unsafe.SizeOf<T>() == 8
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = global::System.Threading.Interlocked.And(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.or#system-threading-interlocked-or-1(-0@-0)
        public static T Or<T>(ref T location1, T value)
            where T : struct
        {
            // Only integer primitive types and enum types backed by integer types are supported.
            // Floating-point types and floating-point backed enums are not supported.
            if (
                (!typeof(T).IsPrimitive && !typeof(T).IsEnum)
                || typeof(T) == typeof(float)
                || typeof(T) == typeof(double)
                || (
                    typeof(T).IsEnum
                    && (
                        typeof(T).GetEnumUnderlyingType() == typeof(float)
                        || typeof(T).GetEnumUnderlyingType() == typeof(double)
                    )
                )
            )
            {
                throw new NotSupportedException(
                    "Only integer primitive types and enums backed by integer types are supported."
                );
            }

            // For all types, use CompareExchange-based implementation since we're polyfilling
            // the generic overload that doesn't exist yet
            if (Unsafe.SizeOf<T>() == 1)
            {
                ref byte loc = ref Unsafe.As<T, byte>(ref location1);
                byte val = Unsafe.As<T, byte>(ref value);
                byte current = loc;
                while (true)
                {
                    byte newValue = (byte)(current | val);
                    byte oldValue = global::System.Threading.Interlocked.CompareExchange<byte>(
                        ref loc,
                        newValue,
                        current
                    );
                    if (oldValue == current)
                    {
                        byte result = oldValue;
                        return Unsafe.As<byte, T>(ref result);
                    }
                    current = oldValue;
                }
            }

            if (Unsafe.SizeOf<T>() == 2)
            {
                ref ushort loc = ref Unsafe.As<T, ushort>(ref location1);
                ushort val = Unsafe.As<T, ushort>(ref value);
                ushort current = loc;
                while (true)
                {
                    ushort newValue = (ushort)(current | val);
                    ushort oldValue = global::System.Threading.Interlocked.CompareExchange<ushort>(
                        ref loc,
                        newValue,
                        current
                    );
                    if (oldValue == current)
                    {
                        ushort result = oldValue;
                        return Unsafe.As<ushort, T>(ref result);
                    }
                    current = oldValue;
                }
            }

            if (Unsafe.SizeOf<T>() == 4)
            {
                ref int loc = ref Unsafe.As<T, int>(ref location1);
                int val = Unsafe.As<T, int>(ref value);
                int result = global::System.Threading.Interlocked.Or(ref loc, val);
                return Unsafe.As<int, T>(ref result);
            }

            // Unsafe.SizeOf<T>() == 8
            {
                ref long loc = ref Unsafe.As<T, long>(ref location1);
                long val = Unsafe.As<T, long>(ref value);
                long result = global::System.Threading.Interlocked.Or(ref loc, val);
                return Unsafe.As<long, T>(ref result);
            }
        }
    }
}
#endif
#endif
