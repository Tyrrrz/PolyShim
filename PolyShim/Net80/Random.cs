#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net80_Random
{
    extension(Random random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.getitems#system-random-getitems-1(-0()-system-int32)
        public T[] GetItems<T>(T[] choices, int length)
        {
            var result = new T[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = choices[random.Next(choices.Length)];
            }
            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.random.shuffle#system-random-shuffle-1(-0())
        public void Shuffle<T>(T[] items)
        {
            for (var i = items.Length - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);
                (items[i], items[j]) = (items[j], items[i]);
            }
        }
    }
}
#endif
