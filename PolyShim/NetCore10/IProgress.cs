#if (NETFRAMEWORK && !NET45_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

namespace System;

// https://learn.microsoft.com/dotnet/api/system.iprogress-1
internal interface IProgress<in T>
{
    void Report(T value);
}
#endif
