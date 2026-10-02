#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NET45_OR_GREATER) || (NETSTANDARD)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.configureawaitoptions
[Flags]
internal enum ConfigureAwaitOptions
{
    None = 0,
    ContinueOnCapturedContext = 1,
    SuppressThrowing = 2,
    ForceYielding = 4,
}
#endif
#endif
