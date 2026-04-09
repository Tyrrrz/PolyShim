#if !FEATURE_TIMEPROVIDER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.itimer
internal interface ITimer : IDisposable
#if !(NETCOREAPP && !NETCOREAPP2_0_OR_GREATER)
        , IAsyncDisposable
#endif
{
    bool Change(TimeSpan dueTime, TimeSpan period);
}
#endif
