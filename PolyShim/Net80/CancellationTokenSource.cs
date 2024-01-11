#if FEATURE_TASK
#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource.cancelasync
    public static Task CancelAsync(this CancellationTokenSource cts)
    {
#if (NETFRAMEWORK && !NET45_OR_GREATER)
        return Task.Factory.StartNew(() => cts.Cancel());
#elif (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
        cts.Cancel();
        return Task.FromResult<object?>(null);
#else
        cts.Cancel();
        return Task.CompletedTask;
#endif
    }
}
#endif
#endif
