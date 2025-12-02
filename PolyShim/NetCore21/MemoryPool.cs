#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.memorypool-1
[ExcludeFromCodeCoverage]
internal partial class MemoryPool<T> : IDisposable
{
    public static MemoryPool<T> Shared { get; } = new();

    public IMemoryOwner<T> Rent(int minBufferSize = -1)
    {
        if (minBufferSize < -1)
            throw new ArgumentOutOfRangeException(nameof(minBufferSize));

        return new MemoryOwner(minBufferSize < 0 ? 0 : minBufferSize);
    }

    public void Dispose() { }
}

internal partial class MemoryPool<T>
{
    private class MemoryOwner(int length) : IMemoryOwner<T>
    {
        public Memory<T> Memory { get; } = new(new T[length]);

        public void Dispose() { }
    }
}
#endif
