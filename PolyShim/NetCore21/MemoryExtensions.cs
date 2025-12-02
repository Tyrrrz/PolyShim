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
    extension<T>(T[]? array)
    {
        public Span<T> AsSpan(int start, int length) => new(array, start, length);

        public Span<T> AsSpan(int start) => array.AsSpan(start, array?.Length ?? 0 - start);

        public Span<T> AsSpan() => array.AsSpan(0);

        public Memory<T> AsMemory(int start, int length) => new(array, start, length);

        public Memory<T> AsMemory(int start) => array.AsMemory(start, array?.Length ?? 0 - start);

        public Memory<T> AsMemory() => array.AsMemory(0);

        public void CopyTo(Span<T> destination) => array.AsSpan().CopyTo(destination);

        public void CopyTo(Memory<T> destination) => array.AsSpan().CopyTo(destination.Span);
    }

    extension<T>(ArraySegment<T>? segment)
    {
        public Span<T> AsSpan() => new(segment?.Array, segment?.Offset ?? 0, segment?.Count ?? 0);
    }

    extension(string text)
    {
        public ReadOnlySpan<char> AsSpan(int start, int length) =>
            new(text.ToCharArray(), start, length);

        public ReadOnlySpan<char> AsSpan(int start) => text.AsSpan(start, text.Length - start);

        public ReadOnlySpan<char> AsSpan() => text.AsSpan(0);

        public ReadOnlyMemory<char> AsMemory(int start, int length) =>
            new(text.ToCharArray(), start, length);

        public ReadOnlyMemory<char> AsMemory(int start) =>
            text.AsMemory(start, text.Length - start);

        public ReadOnlyMemory<char> AsMemory() => text.AsMemory(0);
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
}
#endif
