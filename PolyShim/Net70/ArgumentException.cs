#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Runtime.CompilerServices;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_ArgumentException
{
    extension(ArgumentException)
    {
        // https://learn.microsoft.com/dotnet/api/system.argumentexception.throwifnullorempty
        public static void ThrowIfNullOrEmpty(
            string? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null
        )
        {
            if (argument is null)
                throw new ArgumentNullException(paramName);

            if (string.IsNullOrEmpty(argument))
                throw new ArgumentException("The value cannot be an empty string.", paramName);
        }

        // https://learn.microsoft.com/dotnet/api/system.argumentexception.throwifnullorwhitespace
        public static void ThrowIfNullOrWhiteSpace(
            string? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null
        )
        {
            if (argument is null)
                throw new ArgumentNullException(paramName);

            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException(
                    "The value cannot be an empty string or composed entirely of whitespace.",
                    paramName
                );
            }
        }
    }
}
#endif
