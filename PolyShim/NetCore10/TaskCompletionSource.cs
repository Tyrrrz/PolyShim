#if (NET20_OR_GREATER && !NET46_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

[ExcludeFromCodeCoverage]
internal static class _6721532A201A420BB96E16A5B23DFB4F
{
#if FEATURE_TASK
    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1.trysetcanceled#system-threading-tasks-taskcompletionsource-1-trysetcanceled(system-threading-cancellationtoken)
    public static bool TrySetCanceled<T>(this TaskCompletionSource<T> source, CancellationToken cancellationToken) =>
        source.TrySetException(new OperationCanceledException(cancellationToken));
#endif
}
#endif