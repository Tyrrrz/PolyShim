#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_Stack
{
    extension<T>(Stack<T> stack)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.stack-1.trypeek
        public bool TryPeek([MaybeNullWhen(false)] out T result)
        {
            if (stack.Count == 0)
            {
                result = default;
                return false;
            }

            result = stack.Peek();
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.stack-1.trypop
        public bool TryPop([MaybeNullWhen(false)] out T result)
        {
            if (stack.Count == 0)
            {
                result = default;
                return false;
            }

            result = stack.Pop();
            return true;
        }
    }
}
#endif
