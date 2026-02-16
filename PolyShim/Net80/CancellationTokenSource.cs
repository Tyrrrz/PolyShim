#if FEATURE_TASK
#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net80_CancellationTokenSource
{
    extension(CancellationTokenSource cts)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.cancellationtokensource.cancelasync
        public Task CancelAsync()
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
}
#endif
#endif
