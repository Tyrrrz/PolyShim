#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.threadabortexception
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ThreadAbortException : SystemException
{
    // This exception cannot be instantiated by user code
    private ThreadAbortException() => HResult = unchecked((int)0x80131530);

    public object? ExceptionState => null;
}
#endif
