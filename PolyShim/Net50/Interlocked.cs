#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;
using System.Threading;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net50_Interlocked
{
    extension(Interlocked)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and(system-int32@-system-int32)
        public static int And(ref int location1, int value)
        {
            int current,
                newValue,
                original;
            do
            {
                current = location1;
                newValue = current & value;
                original = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (original != current);

            return original;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and(system-int64@-system-int64)
        public static long And(ref long location1, long value)
        {
            long current,
                newValue,
                original;
            do
            {
                current = location1;
                newValue = current & value;
                original = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (original != current);

            return original;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.or#system-threading-interlocked-or(system-int32@-system-int32)
        public static int Or(ref int location1, int value)
        {
            int current,
                newValue,
                original;
            do
            {
                current = location1;
                newValue = current | value;
                original = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (original != current);

            return original;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.or#system-threading-interlocked-or(system-int64@-system-int64)
        public static long Or(ref long location1, long value)
        {
            long current,
                newValue,
                original;
            do
            {
                current = location1;
                newValue = current | value;
                original = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (original != current);

            return original;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and(system-uint32@-system-uint32)
        public static unsafe uint And(ref uint location1, uint value)
        {
            fixed (uint* ptr = &location1)
            {
                int result = Interlocked.And(ref *(int*)ptr, *(int*)&value);
                return *(uint*)&result;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.and#system-threading-interlocked-and(system-uint64@-system-uint64)
        public static unsafe ulong And(ref ulong location1, ulong value)
        {
            fixed (ulong* ptr = &location1)
            {
                long result = Interlocked.And(ref *(long*)ptr, *(long*)&value);
                return *(ulong*)&result;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.or#system-threading-interlocked-or(system-uint32@-system-uint32)
        public static unsafe uint Or(ref uint location1, uint value)
        {
            fixed (uint* ptr = &location1)
            {
                int result = Interlocked.Or(ref *(int*)ptr, *(int*)&value);
                return *(uint*)&result;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.interlocked.or#system-threading-interlocked-or(system-uint64@-system-uint64)
        public static unsafe ulong Or(ref ulong location1, ulong value)
        {
            fixed (ulong* ptr = &location1)
            {
                long result = Interlocked.Or(ref *(long*)ptr, *(long*)&value);
                return *(ulong*)&result;
            }
        }
    }
}

#endif
