#if NETSTANDARD && !NETSTANDARD1_2_OR_GREATER
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.timercallback
internal delegate void TimerCallback(object? state);
#endif
