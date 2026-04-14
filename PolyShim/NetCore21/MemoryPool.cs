#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.memorypool-1
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal partial class MemoryPool<T> : IDisposable
{
    public int MaxBufferSize => int.MaxValue;

    public IMemoryOwner<T> Rent(int minBufferSize = -1)
    {
        var innerPool = ArrayPool<T>.Shared;
        var buffer = innerPool.Rent(minBufferSize >= 0 ? minBufferSize : 16);

        try
        {
            return new MemoryOwner(innerPool, buffer);
        }
        catch
        {
            innerPool.Return(buffer);
            throw;
        }
    }

    public void Dispose() { }

    public static MemoryPool<T> Shared { get; } = new();
}

internal partial class MemoryPool<T>
{
#if !POLYSHIM_EXCLUDE_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    private class MemoryOwner(ArrayPool<T> pool, T[] buffer) : IMemoryOwner<T>
    {
        public Memory<T> Memory { get; } = new(buffer);

        public void Dispose() => pool.Return(buffer);
    }
}
#endif
