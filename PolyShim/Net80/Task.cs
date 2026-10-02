#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NET45_OR_GREATER) || (NETSTANDARD)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_Task
{
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.configureawait#system-threading-tasks-task-configureawait(system-threading-tasks-configureawaitoptions)
        // ConfigureAwaitOptions.SuppressThrowing and ConfigureAwaitOptions.ForceYielding cannot be
        // emulated without reimplementing the awaiter from scratch, so they are treated as no-ops.
        public ConfiguredTaskAwaitable ConfigureAwait(ConfigureAwaitOptions options) =>
            task.ConfigureAwait((options & ConfigureAwaitOptions.ContinueOnCapturedContext) != 0);
    }

    extension<TResult>(Task<TResult> task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1.configureawait#system-threading-tasks-task-1-configureawait(system-threading-tasks-configureawaitoptions)
        // ConfigureAwaitOptions.SuppressThrowing and ConfigureAwaitOptions.ForceYielding cannot be
        // emulated without reimplementing the awaiter from scratch, so they are treated as no-ops.
        public ConfiguredTaskAwaitable<TResult> ConfigureAwait(ConfigureAwaitOptions options) =>
            task.ConfigureAwait((options & ConfigureAwaitOptions.ContinueOnCapturedContext) != 0);
    }
}
#endif
#endif
