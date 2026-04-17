#if (NETFRAMEWORK && !NET40_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference.
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.paralleloptions
#if !POLYSHIM_INCLUDE_COVERAGE
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
