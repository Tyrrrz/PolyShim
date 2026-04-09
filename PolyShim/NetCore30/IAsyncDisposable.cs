#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading.Tasks;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.iasyncdisposable
internal interface IAsyncDisposable
{
    ValueTask DisposeAsync();
}
#endif
#endif
