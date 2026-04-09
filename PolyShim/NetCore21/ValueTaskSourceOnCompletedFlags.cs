#if FEATURE_TASK
// The compatibility package that provides FEATURE_VALUETASK, doesn't backport this specific type on some target frameworks
#if !FEATURE_VALUETASK || (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK && !NET461_OR_GREATER)
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
