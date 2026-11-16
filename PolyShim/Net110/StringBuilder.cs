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
    extension(StringBuilder builder)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.stringbuilder.movechunks
        // Not zero-copy: contents are copied to this builder and the source is cleared
        public void MoveChunks(StringBuilder source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (ReferenceEquals(builder, source))
                throw new InvalidOperationException("Source and destination must be different.");

            builder.Append(source.ToString());
            source.Clear();
        }
    }
}
#endif
