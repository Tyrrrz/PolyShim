#if !FEATURE_VALUETASK_SOURCES
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

namespace System.Threading.Tasks.Sources;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.valuetasksourceoncompletedflag
[Flags]
internal enum ValueTaskSourceOnCompletedFlags
{
    None = 0,
    UseSchedulingContext = 1,
    FlowExecutionContext = 2,
}
#endif
#endif
