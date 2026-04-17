#if FEATURE_TASK
// IValueTaskSource is provided via the System.Threading.Tasks.Extensions NuGet package when available.
#if !FEATURE_VALUETASK_SOURCES
#nullable enable
#pragma warning disable CS0436

using System;

namespace System.Threading.Tasks.Sources;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.ivaluetasksource
internal interface IValueTaskSource
{
    ValueTaskSourceStatus GetStatus(short token);

    void OnCompleted(
        Action<object?> continuation,
        object? state,
        short token,
        ValueTaskSourceOnCompletedFlags flags
    );

    void GetResult(short token);
}

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.ivaluetasksource-1
internal interface IValueTaskSource<out TResult>
{
    ValueTaskSourceStatus GetStatus(short token);

    void OnCompleted(
        Action<object?> continuation,
        object? state,
        short token,
        ValueTaskSourceOnCompletedFlags flags
    );

    TResult GetResult(short token);
}
#endif
#endif
