#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_ObjectDisposedException
{
    extension(ObjectDisposedException)
    {
        // https://learn.microsoft.com/dotnet/api/system.objectdisposedexception.throwif#system-objectdisposedexception-throwif(system-boolean-system-type)
        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
        {
            if (condition)
                throw new ObjectDisposedException(type.FullName);
        }

        // https://learn.microsoft.com/dotnet/api/system.objectdisposedexception.throwif#system-objectdisposedexception-throwif(system-boolean-system-object)
        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance) =>
            ThrowIf(condition, instance.GetType());
    }
}
#endif
