#if !FEATURE_TIMEPROVIDER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.itimer
internal interface ITimer : IDisposable
    // IAsyncDisposable is provided via the Microsoft.Bcl.AsyncInterfaces NuGet package.
#if FEATURE_ASYNCINTERFACES
        , IAsyncDisposable
#endif
{
    bool Change(TimeSpan dueTime, TimeSpan period);
}
#endif
