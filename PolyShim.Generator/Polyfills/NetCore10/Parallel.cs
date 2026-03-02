#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Parallel;
