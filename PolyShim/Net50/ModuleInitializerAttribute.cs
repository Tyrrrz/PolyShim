#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.moduleinitializerattribute
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ModuleInitializerAttribute : Attribute;
#endif
