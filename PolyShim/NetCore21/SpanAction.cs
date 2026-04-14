#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.spanaction-2
internal delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);
#endif
