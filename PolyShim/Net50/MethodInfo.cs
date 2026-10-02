#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_MethodInfo
{
    extension(MethodInfo method)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.methodinfo.createdelegate#system-reflection-methodinfo-createdelegate-1
        public TDelegate CreateDelegate<TDelegate>()
            where TDelegate : Delegate =>
#if NETFRAMEWORK && !NET45_OR_GREATER
            (TDelegate)(object)Delegate.CreateDelegate(typeof(TDelegate), method);
#else
            (TDelegate)(object)method.CreateDelegate(typeof(TDelegate));
#endif

        // https://learn.microsoft.com/dotnet/api/system.reflection.methodinfo.createdelegate#system-reflection-methodinfo-createdelegate-1(system-object)
        public TDelegate CreateDelegate<TDelegate>(object? target)
            where TDelegate : Delegate =>
#if NETFRAMEWORK && !NET45_OR_GREATER
            (TDelegate)(object)Delegate.CreateDelegate(typeof(TDelegate), target, method);
#else
            (TDelegate)(object)method.CreateDelegate(typeof(TDelegate), target);
#endif
    }
}
#endif
