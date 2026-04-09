#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_MatchCollection
{
    // MatchCollection started implementing IEnumerable<Match> in .NET Core 2.0,
    // but we can't implement an interface with extension methods.
    // So the best we can do is provide some commonly used polyfills for IEnumerable<Match>.
    extension(MatchCollection matchCollection)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.matchcollection.system-collections-generic-ienumerable-system-text-regularexpressions-match--getenumerator
        public IEnumerable<Match> AsEnumerable() => matchCollection.Cast<Match>();

        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.matchcollection.system-collections-generic-ienumerable-system-text-regularexpressions-match--getenumerator
        public IEnumerator<Match> GetEnumerator() => matchCollection.AsEnumerable().GetEnumerator();

        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.matchcollection.system-collections-generic-ienumerable-system-text-regularexpressions-match--getenumerator
        public Match[] ToArray() => matchCollection.AsEnumerable().ToArray();
    }
}
#endif
