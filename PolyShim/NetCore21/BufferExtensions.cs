#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_BufferExtensions
{
    extension<T>(ReadOnlySequence<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.buffers.buffersextensions.copyto?system-buffers-buffersextensions-copyto-1(system-buffers-readonlysequence((-0))-system-span((-0)))
        public void CopyTo(Span<T> destination)
        {
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException(nameof(destination));

            source.Span.CopyTo(destination);
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.buffersextensions.toarray?system-buffers-buffersextensions-toarray-1(system-buffers-readonlysequence((-0)))
        public T[] ToArray()
        {
            if (source.IsEmpty)
                return new T[0];

            var array = new T[(int)source.Length];
            source.CopyTo(array);

            return array;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.buffersextensions.positionof?system-buffers-buffersextensions-positionof-1(system-buffers-readonlysequence((-0))-0)
        public SequencePosition? PositionOf(T value)
        {
            var span = source.Span;

            for (var i = 0; i < span.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(span[i], value))
                    return source.GetPosition(i, source.Start);
            }

            return null;
        }
    }
}
#endif
