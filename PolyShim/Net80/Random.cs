#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;

internal static partial class PolyfillExtensions
{
    extension(Random random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.getitems#system-random-getitems-1(-0()-system-int32)
        public T[] GetItems<T>(T[] choices, int length)
        {
            var result = new T[length];
            var selectedIndices = new HashSet<int>();

            while (selectedIndices.Count < length)
            {
                var index = random.Next(choices.Length);
                if (selectedIndices.Add(index))
                {
                    result[selectedIndices.Count - 1] = choices[index];
                }
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
