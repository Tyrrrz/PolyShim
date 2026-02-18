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
        // Non-generic And/Or methods for int and long
        public static int And(ref int location1, int value)
        {
            int current = location1;
            int newValue;
            do
            {
                newValue = current & value;
                current = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (current != newValue);

            return newValue;
        }

        public static long And(ref long location1, long value)
        {
            long current = location1;
            long newValue;
            do
            {
                newValue = current & value;
                current = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (current != newValue);

            return newValue;
        }

        public static int Or(ref int location1, int value)
        {
            int current = location1;
            int newValue;
            do
            {
                newValue = current | value;
                current = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (current != newValue);

            return newValue;
        }

        public static long Or(ref long location1, long value)
        {
            long current = location1;
            long newValue;
            do
            {
                newValue = current | value;
                current = Interlocked.CompareExchange(ref location1, newValue, current);
            } while (current != newValue);

            return newValue;
        }

        // uint/ulong overloads use unsafe pointers
        public static unsafe uint And(ref uint location1, uint value)
        {
            fixed (uint* ptr = &location1)
            {
                int result = Interlocked.And(ref *(int*)ptr, *(int*)&value);
                return *(uint*)&result;
            }
        }

        public static unsafe ulong And(ref ulong location1, ulong value)
        {
            fixed (ulong* ptr = &location1)
            {
                long result = Interlocked.And(ref *(long*)ptr, *(long*)&value);
                return *(ulong*)&result;
            }
        }

        public static unsafe uint Or(ref uint location1, uint value)
        {
            fixed (uint* ptr = &location1)
            {
                int result = Interlocked.Or(ref *(int*)ptr, *(int*)&value);
                return *(uint*)&result;
            }
        }

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
