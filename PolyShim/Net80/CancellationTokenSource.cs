#if FEATURE_TASK
#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
