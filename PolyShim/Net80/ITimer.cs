#if !FEATURE_TIMEPROVIDER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.itimer
internal interface ITimer : IDisposable
        // Task infrastructure is required for async method support
#if FEATURE_TASK
        , IAsyncDisposable
#endif
{
    bool Change(TimeSpan dueTime, TimeSpan period);
}
#endif
