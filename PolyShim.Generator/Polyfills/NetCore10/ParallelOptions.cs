#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

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
