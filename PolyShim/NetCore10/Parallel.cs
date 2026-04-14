#if (NETFRAMEWORK && !NET40_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Parallel;
#endif
