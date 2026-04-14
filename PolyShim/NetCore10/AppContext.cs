#if (NETFRAMEWORK && !NET47_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_6_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_AppContext
{
    extension(AppContext)
    {
        // No reflection on .NET Standard prior to 1.5
#if !NETSTANDARD || NETSTANDARD1_5_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.appcontext.targetframeworkname
        public static string? TargetFrameworkName =>
            (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly())
                .GetCustomAttributes(true)
                .OfType<TargetFrameworkAttribute>()
                .FirstOrDefault()
                ?.FrameworkName;
#endif
    }
}

// Some frameworks lack the type altogether
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
namespace System
{
    // https://learn.microsoft.com/dotnet/api/system.appcontext
#if !POLYSHIM_INCLUDE_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal static class AppContext;
}
#endif
#endif
