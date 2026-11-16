#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Text;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_StringBuilder
{
    extension(StringBuilder builder)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.stringbuilder.clear
        public StringBuilder Clear()
        {
            builder.Length = 0;
            return builder;
        }
    }
}
#endif
