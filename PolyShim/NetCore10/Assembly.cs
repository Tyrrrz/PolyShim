#if (NETFRAMEWORK && !NET45_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_Assembly
{
    extension(Assembly assembly)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-assembly)
        public T? GetCustomAttribute<T>()
            where T : Attribute =>
            (T?)assembly.GetCustomAttributes(typeof(T), false).FirstOrDefault();
    }
}
#endif
