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

namespace System.Reflection;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_CustomAttributeExtensions
{
    extension(Module module)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-module)
        public T? GetCustomAttribute<T>()
            where T : Attribute =>
            (T?)module.GetCustomAttributes(typeof(T), false).FirstOrDefault();
    }

    extension(Assembly assembly)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-assembly)
        public T? GetCustomAttribute<T>()
            where T : Attribute =>
            (T?)assembly.GetCustomAttributes(typeof(T), false).FirstOrDefault();
    }

    extension(MemberInfo member)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-memberinfo-system-boolean)
        public T? GetCustomAttribute<T>(bool inherit)
            where T : Attribute =>
            (T?)member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();

        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-memberinfo)
        public T? GetCustomAttribute<T>()
            where T : Attribute =>
            member.GetCustomAttribute<T>(true);
    }

    extension(ParameterInfo parameter)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-parameterinfo-system-boolean)
        public T? GetCustomAttribute<T>(bool inherit)
            where T : Attribute =>
            (T?)parameter.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();

        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-parameterinfo)
        public T? GetCustomAttribute<T>()
            where T : Attribute =>
            parameter.GetCustomAttribute<T>(true);
    }
}
#endif
