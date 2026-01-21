#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System;

internal static partial class PolyfillExtensions
{
    extension<T>(T[]? array)
    {
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan-1(-0()-system-int32-system-int32)
        public Span<T> AsSpan(int start, int length) => new(array, start, length);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan-1(-0()-system-int32)
        public Span<T> AsSpan(int start) => array.AsSpan(start, (array?.Length ?? 0) - start);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan-1(-0())
        public Span<T> AsSpan() => array.AsSpan(0);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asmemory#system-memoryextensions-asmemory-1(-0()-system-int32-system-int32)
        public Memory<T> AsMemory(int start, int length) => new(array, start, length);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asmemory#system-memoryextensions-asmemory-1(-0()-system-int32)
        public Memory<T> AsMemory(int start) => array.AsMemory(start, (array?.Length ?? 0) - start);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asmemory#system-memoryextensions-asmemory-1(-0())
        public Memory<T> AsMemory() => array.AsMemory(0);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.copyto#system-memoryextensions-copyto-1(-0()-system-span((-0)))
        public void CopyTo(Span<T> destination) => array.AsSpan().CopyTo(destination);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.copyto#system-memoryextensions-copyto-1(-0()-system-memory((-0)))
        public void CopyTo(Memory<T> destination) => array.AsSpan().CopyTo(destination.Span);
    }

    extension<T>(ArraySegment<T>? segment)
    {
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan-1(system-arraysegment((-0)))
        public Span<T> AsSpan() => new(segment?.Array, segment?.Offset ?? 0, segment?.Count ?? 0);
    }

    extension(string? text)
    {
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan(system-string-system-int32-system-int32)
        public ReadOnlySpan<char> AsSpan(int start, int length) =>
            new(text?.ToCharArray(), start, length);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan(system-string-system-int32)
        public ReadOnlySpan<char> AsSpan(int start) =>
            text.AsSpan(start, (text?.Length ?? 0) - start);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asspan#system-memoryextensions-asspan(system-string)
        public ReadOnlySpan<char> AsSpan() => text.AsSpan(0);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asmemory#system-memoryextensions-asmemory(system-string-system-int32-system-int32)
        public ReadOnlyMemory<char> AsMemory(int start, int length) =>
            new(text?.ToCharArray(), start, length);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asmemory#system-memoryextensions-asmemory(system-string-system-int32)
        public ReadOnlyMemory<char> AsMemory(int start) =>
            text.AsMemory(start, (text?.Length ?? 0) - start);

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.asmemory#system-memoryextensions-asmemory(system-string)
        public ReadOnlyMemory<char> AsMemory() => text.AsMemory(0);
    }

    extension<T>(Span<T> span)
        where T : IEquatable<T>
    {
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.contains#system-memoryextensions-contains-1(system-span((-0))-0)
        public bool Contains(T value)
        {
            foreach (var item in span)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.indexof#system-memoryextensions-indexof-1(system-span((-0))-0)
        public int IndexOf(T value)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i].Equals(value))
                    return i;
            }

            return -1;
        }

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.sequenceequal#system-memoryextensions-sequenceequal-1(system-span((-0))-system-readonlyspan((-0)))
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

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.reverse#system-memoryextensions-reverse-1(system-span((-0)))
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
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.contains#system-memoryextensions-contains-1(system-readonlyspan((-0))-0)
        public bool Contains(T value)
        {
            foreach (var item in span)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.indexof#system-memoryextensions-indexof-1(system-readonlyspan((-0))-0)
        public int IndexOf(T value)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i].Equals(value))
                    return i;
            }

            return -1;
        }

        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.sequenceequal#system-memoryextensions-sequenceequal-1(system-readonlyspan((-0))-system-readonlyspan((-0)))
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
