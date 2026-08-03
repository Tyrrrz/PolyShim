#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_TimeOnly
{
    extension(TimeOnly time)
    {
        // https://learn.microsoft.com/dotnet/api/system.timeonly.deconstruct#system-timeonly-deconstruct(system-int32@-system-int32@)
        public void Deconstruct(out int hour, out int minute)
        {
            hour = time.Hour;
            minute = time.Minute;
        }

        // https://learn.microsoft.com/dotnet/api/system.timeonly.deconstruct#system-timeonly-deconstruct(system-int32@-system-int32@-system-int32@)
        public void Deconstruct(out int hour, out int minute, out int second)
        {
            hour = time.Hour;
            minute = time.Minute;
            second = time.Second;
        }

        // https://learn.microsoft.com/dotnet/api/system.timeonly.deconstruct#system-timeonly-deconstruct(system-int32@-system-int32@-system-int32@-system-int32@)
        public void Deconstruct(out int hour, out int minute, out int second, out int millisecond)
        {
            hour = time.Hour;
            minute = time.Minute;
            second = time.Second;
            millisecond = time.Millisecond;
        }
    }
}
#endif
