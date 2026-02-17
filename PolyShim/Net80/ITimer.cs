#if !FEATURE_TIMEPROVIDER && ((NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD))
// Only include ITimer if we have Task support and are on .NET Standard 2.0+, .NET Core 2.0+, or .NET Framework 4.6.1+
#if FEATURE_TASK && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.itimer
internal interface ITimer : IDisposable
#if FEATURE_ASYNCINTERFACES
    ,
    IAsyncDisposable
#endif
{
    bool Change(TimeSpan dueTime, TimeSpan period);
}
#endif
#endif
