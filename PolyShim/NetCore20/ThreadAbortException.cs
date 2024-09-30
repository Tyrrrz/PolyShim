﻿#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

// https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadabortexception
[ExcludeFromCodeCoverage]
internal class ThreadAbortException : SystemException
{
    // This exception cannot be instantiated by user code
    private ThreadAbortException() => HResult = unchecked((int)0x80131530);

    public object? ExceptionState => null;
}
#endif
