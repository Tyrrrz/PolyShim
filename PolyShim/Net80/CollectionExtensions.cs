#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_CollectionExtensions
{
    extension<T>(List<T> list)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.addrange
        public void AddRange(ReadOnlySpan<T> source)
        {
            foreach (var item in source)
                list.Add(item);
        }

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.insertrange
        public void InsertRange(int index, ReadOnlySpan<T> source)
        {
            for (var i = 0; i < source.Length; i++)
                list.Insert(index + i, source[i]);
        }

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.copyto
        public void CopyTo(Span<T> destination)
        {
            if (destination.Length < list.Count)
                throw new ArgumentException("Destination is too short.", nameof(destination));

            for (var i = 0; i < list.Count; i++)
                destination[i] = list[i];
        }
    }
}
#endif
