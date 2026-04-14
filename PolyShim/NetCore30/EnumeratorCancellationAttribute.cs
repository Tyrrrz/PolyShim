#if !FEATURE_ASYNCINTERFACES
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.enumeratorcancellationattribute
[AttributeUsage(AttributeTargets.Parameter)]
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class EnumeratorCancellationAttribute : Attribute;
#endif
