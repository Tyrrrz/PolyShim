#if (NETFRAMEWORK && !NET45_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.callermembernameattribute
[AttributeUsage(AttributeTargets.Parameter)]
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class CallerMemberNameAttribute : Attribute;
#endif
