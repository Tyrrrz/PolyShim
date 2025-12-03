#if (NETFRAMEWORK && !NET45_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.callermembernameattribute
[AttributeUsage(AttributeTargets.Parameter)]
[ExcludeFromCodeCoverage]
internal class CallerMemberNameAttribute : Attribute;
#endif
