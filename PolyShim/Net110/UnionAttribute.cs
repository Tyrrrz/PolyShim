#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.unionattribute
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class UnionAttribute : Attribute;
#endif
