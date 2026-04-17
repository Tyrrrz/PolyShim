#if !FEATURE_ASYNCINTERFACES
// Task infrastructure is required for async method return types
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Threading.Tasks;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.iasyncdisposable
internal interface IAsyncDisposable
{
    ValueTask DisposeAsync();
}
#endif
#endif
