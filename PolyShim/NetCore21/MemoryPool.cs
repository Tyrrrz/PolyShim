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

    public IMemoryOwner<T> Rent(int minimumLength = -1)
    {
        if (minimumLength < -1)
            throw new ArgumentOutOfRangeException(nameof(minimumLength));

        return new MemoryOwner(minimumLength < 0 ? 0 : minimumLength);
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
