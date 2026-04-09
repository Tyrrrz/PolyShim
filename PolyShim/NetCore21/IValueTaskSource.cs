#if FEATURE_TASK
// Compatibility package that provides FEATURE_VALUETASK doesn't backport this specific type on some target frameworks
#if !FEATURE_VALUETASK || (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK && !NET461_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

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
