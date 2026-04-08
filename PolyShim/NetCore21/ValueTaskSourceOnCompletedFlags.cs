#if !FEATURE_VALUETASK
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

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
