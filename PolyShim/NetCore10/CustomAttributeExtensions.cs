#if (NETFRAMEWORK && !NET45_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Reflection;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_CustomAttributeExtensions
{
    extension(Module module)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-module)
        public T? GetCustomAttribute<T>()
            where T : Attribute => (T?)Attribute.GetCustomAttribute(module, typeof(T));
    }

    extension(Assembly assembly)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-assembly)
        public T? GetCustomAttribute<T>()
            where T : Attribute => (T?)Attribute.GetCustomAttribute(assembly, typeof(T));
    }

    extension(MemberInfo member)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-memberinfo-system-boolean)
        public T? GetCustomAttribute<T>(bool inherit)
            where T : Attribute => (T?)Attribute.GetCustomAttribute(member, typeof(T), inherit);

        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-memberinfo)
        public T? GetCustomAttribute<T>()
            where T : Attribute => member.GetCustomAttribute<T>(true);
    }

    extension(ParameterInfo parameter)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-parameterinfo-system-boolean)
        public T? GetCustomAttribute<T>(bool inherit)
            where T : Attribute => (T?)Attribute.GetCustomAttribute(parameter, typeof(T), inherit);

        // https://learn.microsoft.com/dotnet/api/system.reflection.customattributeextensions.getcustomattribute#system-reflection-customattributeextensions-getcustomattribute-1(system-reflection-parameterinfo)
        public T? GetCustomAttribute<T>()
            where T : Attribute => parameter.GetCustomAttribute<T>(true);
    }
}
#endif
