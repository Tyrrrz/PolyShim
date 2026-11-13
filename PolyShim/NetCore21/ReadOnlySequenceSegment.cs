#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.readonlysequencesegment-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal abstract class ReadOnlySequenceSegment<T>
{
    public ReadOnlyMemory<T> Memory { get; protected set; }

    public ReadOnlySequenceSegment<T>? Next { get; protected set; }

    public long RunningIndex { get; protected set; }
}
#endif