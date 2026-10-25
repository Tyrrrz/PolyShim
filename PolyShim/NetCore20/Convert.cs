#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK && !NETFRAMEWORK2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_Convert
{
    extension(Convert)
    {
        // https://learn.microsoft.com/dotnet/api/system.convert.tobase64string?system-convert-tobase64string(system-byte()-system-int32-system-int32-system-base64formattingoptions)
        public static string ToBase64String(
            byte[] inArray,
            int offset,
            int length,
            Base64FormattingOptions options = Base64FormattingOptions.None
        )
        {
            var buffer = new StringBuilder(Convert.ToBase64String(inArray, offset, length));

            if (options.HasFlag(Base64FormattingOptions.InsertLineBreaks))
            {
                for (int i = 76; i < buffer.Length; i += 77)
                    buffer.Insert(i, Environment.NewLine);
            }

            return buffer.ToString();
        }

        // https://learn.microsoft.com/dotnet/api/system.convert.tobase64chararray?system-convert-tobase64chararray(system-byte()-system-int32-system-int32-system-char()-system-int32-system-base64formattingoptions)
        public static int ToBase64CharArray(
            byte[] inArray,
            int offsetIn,
            int length,
            char[] outArray,
            int offsetOut,
            Base64FormattingOptions options
        )
        {
            var base64 = ToBase64String(inArray, offsetIn, length, options);
            base64.CopyTo(0, outArray, offsetOut, base64.Length);

            return base64.Length;
        }
    }
}
#endif
