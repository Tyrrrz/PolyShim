#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace System;

// https://learn.microsoft.com/en-us/dotnet/api/system.aggregateexception
[ExcludeFromCodeCoverage]
internal class AggregateException : Exception
{
    public ReadOnlyCollection<Exception> InnerExceptions { get; } = new(new Exception[0]);

    public AggregateException(string? message, params Exception[] innerExceptions)
        : base(message, innerExceptions.FirstOrDefault())
    {
        InnerExceptions = new ReadOnlyCollection<Exception>(innerExceptions);
    }

    public AggregateException(string? message, IEnumerable<Exception> innerExceptions)
        : this(message, innerExceptions.ToArray()) { }

    public AggregateException(string? message, Exception innerException)
        : this(message, new[] { innerException }) { }

    public AggregateException(params Exception[] innerExceptions)
        : this("One or more errors occurred.", innerExceptions) { }

    public AggregateException(IEnumerable<Exception> innerExceptions)
        : this("One or more errors occurred.", innerExceptions) { }

    public AggregateException(string? message)
        : this(message, new Exception[0]) { }

    public AggregateException()
        : this("One or more errors occurred.", new Exception[0]) { }

    public AggregateException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public AggregateException Flatten()
    {
        var innerExceptions = new List<Exception>();

        foreach (var exception in InnerExceptions)
        {
            if (exception is AggregateException aggregateException)
            {
                innerExceptions.AddRange(aggregateException.Flatten().InnerExceptions);
            }
            else
            {
                innerExceptions.Add(exception);
            }
        }

        return new AggregateException(innerExceptions);
    }

    public void Handle(Func<Exception, bool> predicate)
    {
        foreach (var exception in InnerExceptions)
        {
            if (exception is AggregateException aggregateException)
            {
                aggregateException.Handle(predicate);
            }
            else if (predicate(exception))
            {
                throw exception;
            }
        }
    }

    public override string ToString()
    {
        var text = base.ToString();

        for (var i = 0; i < InnerExceptions.Count; i++)
        {
            text = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}---> (Inner Exception #{2}) {3}{4}{5}",
                text,
                Environment.NewLine,
                i,
                InnerExceptions[i].ToString(),
                "<---",
                Environment.NewLine
            );
        }

        return text;
    }
}
#endif
