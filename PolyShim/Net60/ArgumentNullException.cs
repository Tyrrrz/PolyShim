#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Runtime.CompilerServices;

internal static partial class PolyfillExtensions
{
    extension(ArgumentNullException)
    {
        // https://learn.microsoft.com/dotnet/api/system.argumentnullexception.throwifnull#system-argumentnullexception-throwifnull(system-object-system-string)
        public static void ThrowIfNull(
            object? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null
        )
        {
            if (argument is null)
                throw new ArgumentNullException(paramName);
        }
    }
}
#endif
