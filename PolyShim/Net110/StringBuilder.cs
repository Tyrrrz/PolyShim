#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_StringBuilder
{
    extension(StringBuilder)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.stringbuilder.movechunks
        // Not zero-copy: contents are copied to a new builder and the source is cleared
        public static StringBuilder MoveChunks(StringBuilder source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var builder = new StringBuilder(source.ToString());
            source.Length = 0;
            return builder;
        }
    }
}
#endif
