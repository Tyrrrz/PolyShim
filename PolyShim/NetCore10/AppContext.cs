#if (NETFRAMEWORK && !NET47_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_6_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

internal static partial class PolyfillExtensions
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
    [ExcludeFromCodeCoverage]
    internal static class AppContext;
}
#endif
#endif
