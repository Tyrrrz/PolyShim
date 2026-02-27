#if FEATURE_TASK
#if (NETFRAMEWORK && !NET40_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.paralleloptions
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ParallelOptions
{
    public CancellationToken CancellationToken { get; set; }

    public int MaxDegreeOfParallelism { get; set; } = -1;

    public TaskScheduler? TaskScheduler { get; set; }
}
#endif
#endif
