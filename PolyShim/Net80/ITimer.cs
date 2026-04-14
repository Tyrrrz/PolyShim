#if !FEATURE_TIMEPROVIDER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.itimer
internal interface ITimer : IDisposable
#if FEATURE_ASYNCINTERFACES
        , IAsyncDisposable
#endif
{
    bool Change(TimeSpan dueTime, TimeSpan period);
}
#endif
