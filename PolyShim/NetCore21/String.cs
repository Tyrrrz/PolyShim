#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_String
{
    extension(string str)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.contains#system-string-contains(system-char-system-stringcomparison)
        public bool Contains(char c, StringComparison comparison) =>
            str.Contains(c.ToString(), comparison);

        // https://learn.microsoft.com/dotnet/api/system.string.contains#system-string-contains(system-string-system-stringcomparison)
        public bool Contains(string sub, StringComparison comparison) =>
            str.IndexOf(sub, comparison) >= 0;
    }

    extension(string)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.create#system-string-create-1(system-int32--0-system-buffers-spanaction(-system-char--0))
        public static string Create<TState>(
            int length,
            TState state,
            SpanAction<char, TState> action
        )
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            if (length == 0)
                return string.Empty;

            var chars = new char[length];
            action(chars, state);
            return new string(chars);
        }
    }
}
#endif
