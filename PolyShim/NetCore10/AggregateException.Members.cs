#if NETFRAMEWORK && NET40_OR_GREATER && !NET45_OR_GREATER
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_AggregateException
{
    extension(AggregateException exception)
    {
        // https://learn.microsoft.com/dotnet/api/system.aggregateexception.handle
        public void Handle(Func<Exception, bool> predicate)
        {
            var unhandled = new List<Exception>();

            foreach (var exception in exception.InnerExceptions)
            {
                if (!predicate(exception))
                    unhandled.Add(exception);
            }

            if (unhandled.Count > 0)
                throw new AggregateException(exception.Message, unhandled);
        }
    }
}
#endif
