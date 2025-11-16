#if FEATURE_TASK
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    private static readonly Task _completedTask = Task.FromResult(0);

    extension(Task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.completedtask
        public static Task CompletedTask => _completedTask;

#if NETFRAMEWORK && !NET45_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.fromresult
        public static Task<T?> FromResult<T>(T? result)
        {
            var tcs = new TaskCompletionSource<T?>();
            tcs.TrySetResult(result);

            return tcs.Task;
        }
#endif
    }
}
#endif
#endif
