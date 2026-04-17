#if FEATURE_TASK
// IValueTaskSource is provided via the System.Threading.Tasks.Extensions NuGet package when available.
#if !FEATURE_VALUETASK_SOURCES
#nullable enable
#pragma warning disable CS0436

namespace System.Threading.Tasks.Sources;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.valuetasksourcestatus
internal enum ValueTaskSourceStatus
{
    Pending = 0,
    Succeeded = 1,
    Faulted = 2,
    Canceled = 3,
}
#endif
#endif
