#if !FEATURE_ASYNCINTERFACES && FEATURE_TASK && !(NETCOREAPP && !NETCOREAPP2_0_OR_GREATER)
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
