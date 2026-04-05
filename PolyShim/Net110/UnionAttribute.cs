#if !FEATURE_UNIONTYPES
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://devblogs.microsoft.com/dotnet/csharp-15-union-types/
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class UnionAttribute : Attribute { }
#endif
