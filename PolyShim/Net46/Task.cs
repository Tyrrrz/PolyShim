#if FEATURE_TASK
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net46_Task
{
    extension(Task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.fromcanceled
        public static Task FromCanceled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetCanceled();
            return tcs.Task;
        }
    }
}
#endif
#endif
