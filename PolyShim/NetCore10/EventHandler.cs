#if NETFRAMEWORK && !NET45_OR_GREATER
#nullable enable
#pragma warning disable CS0436

namespace System;

// https://learn.microsoft.com/dotnet/api/system.eventhandler-1
internal delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
#endif
