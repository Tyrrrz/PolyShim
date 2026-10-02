#if !FEATURE_ASYNCINTERFACES
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
