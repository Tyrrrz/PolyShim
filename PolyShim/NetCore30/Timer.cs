#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_Timer
{
    extension(Timer timer)
    {
#if FEATURE_ASYNCINTERFACES
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
