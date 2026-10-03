#if NETFRAMEWORK && !NET45_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;

namespace System.Reflection;

internal static class MemberPolyfills_NetCore10_MethodInfo
{
    extension(MethodInfo method)
    {
        // https://learn.microsoft.com/dotnet/api/system.reflection.methodinfo.createdelegate#system-reflection-methodinfo-createdelegate(system-type)
        public Delegate CreateDelegate(Type delegateType) =>
            Delegate.CreateDelegate(delegateType, method);

        // https://learn.microsoft.com/dotnet/api/system.reflection.methodinfo.createdelegate#system-reflection-methodinfo-createdelegate(system-type-system-object)
        public Delegate CreateDelegate(Type delegateType, object? target) =>
            Delegate.CreateDelegate(delegateType, target, method);
    }
}
#endif
