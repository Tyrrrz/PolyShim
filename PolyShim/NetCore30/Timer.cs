#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_Timer
{
    extension(Timer timer)
    {
        // Task infrastructure is required for async method support
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.threading.timer.disposeasync
        public ValueTask DisposeAsync()
        {
            timer.Dispose();
            return default;
        }
#endif
    }
}
#endif
