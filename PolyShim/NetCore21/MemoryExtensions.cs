#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.memoryextensions
[ExcludeFromCodeCoverage]
internal static class MemoryExtensions
{
    extension<T>(T[] array)
    {
        public Span<T> AsSpan() => new(array);

        public Span<T> AsSpan(int start, int length) => new(array, start, length);

        public Memory<T> AsMemory() => new(array);

        public Memory<T> AsMemory(int start) => new(array, start, array.Length - start);

        public Memory<T> AsMemory(int start, int length) => new(array, start, length);

        public void CopyTo(Span<T> destination) => array.AsSpan().CopyTo(destination);

        public void CopyTo(Memory<T> destination) => array.AsSpan().CopyTo(destination.Span);
    }

    extension<T>(ArraySegment<T> segment)
    {
        public Span<T> AsSpan() => new(segment.Array!, segment.Offset, segment.Count);
    }

    extension<T>(Span<T> span)
        where T : IEquatable<T>
    {
        public bool Contains(T value)
        {
            foreach (var item in span)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }

        public int IndexOf(T value)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i].Equals(value))
                    return i;
            }

            return -1;
        }

        public bool SequenceEqual(ReadOnlySpan<T> other)
        {
            if (span.Length != other.Length)
                return false;

            for (var i = 0; i < span.Length; i++)
            {
                if (!span[i].Equals(other[i]))
                    return false;
            }

            return true;
        }

        public void Reverse()
        {
            var i = 0;
            var j = span.Length - 1;

            while (i < j)
            {
                (span[i], span[j]) = (span[j], span[i]);
                i++;
                j--;
            }
        }
    }

    extension<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        public bool Contains(T value)
        {
            foreach (var item in span)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }

        public int IndexOf(T value)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i].Equals(value))
                    return i;
            }

            return -1;
        }

        public bool SequenceEqual(ReadOnlySpan<T> other)
        {
            if (span.Length != other.Length)
                return false;

            for (var i = 0; i < span.Length; i++)
            {
                if (!span[i].Equals(other[i]))
                    return false;
            }

            return true;
        }
    }

    extension(string text)
    {
        public ReadOnlySpan<char> AsSpan() => new(text.ToCharArray());

        public ReadOnlySpan<char> AsSpan(int start) =>
            new(text.ToCharArray(), start, text.Length - start);

        public ReadOnlySpan<char> AsSpan(int start, int length) =>
            new(text.ToCharArray(), start, length);

        public ReadOnlyMemory<char> AsMemory() => new(text.ToCharArray());

        public ReadOnlyMemory<char> AsMemory(int start) =>
            new(text.ToCharArray(), start, text.Length - start);

        public ReadOnlyMemory<char> AsMemory(int start, int length) =>
            new(text.ToCharArray(), start, length);
    }
}
#endif
