#if NETSTANDARD && !NETSTANDARD1_2_OR_GREATER
#nullable enable
#pragma warning disable CS0436

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.timercallback
internal delegate void TimerCallback(object? state);
#endif
