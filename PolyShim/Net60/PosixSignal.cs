#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.posixsignal
internal enum PosixSignal
{
    SIGHUP = -1,
    SIGINT = -2,
    SIGQUIT = -3,
    SIGTERM = -4,
    SIGCHLD = -5,
    SIGCONT = -6,
    SIGWINCH = -7,
    SIGTTIN = -8,
    SIGTTOU = -9,
    SIGTSTP = -10,
}
#endif
