#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.imemoryowner-1
internal interface IMemoryOwner<T> : IDisposable
{
    Memory<T> Memory { get; }
}
#endif
