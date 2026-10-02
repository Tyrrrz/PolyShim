#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asynciteratorstatemachineattribute
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal class AsyncIteratorStateMachineAttribute(Type stateMachineType)
    : StateMachineAttribute(stateMachineType);
#endif
#endif
