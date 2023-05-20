﻿#if FEATURE_TASK
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1.trysetcanceled#system-threading-tasks-taskcompletionsource-1-trysetcanceled(system-threading-cancellationtoken)
    public static bool TrySetCanceled<T>(this TaskCompletionSource<T> source, CancellationToken cancellationToken) =>
        source.TrySetException(new OperationCanceledException(cancellationToken));
}
#endif
#endif