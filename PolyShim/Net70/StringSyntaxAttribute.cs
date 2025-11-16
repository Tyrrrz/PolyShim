#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.stringsyntaxattribute
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
[ExcludeFromCodeCoverage]
internal partial class StringSyntaxAttribute : Attribute
{
    public string Syntax { get; }

    public object?[] Arguments { get; }

    public StringSyntaxAttribute(string syntax, params object?[] arguments)
    {
        Syntax = syntax;
        Arguments = arguments;
    }

    public StringSyntaxAttribute(string syntax)
        : this(syntax, []) { }
}

internal partial class StringSyntaxAttribute
{
    public const string CompositeFormat = nameof(CompositeFormat);

    public const string DateOnlyFormat = nameof(DateOnlyFormat);

    public const string DateTimeFormat = nameof(DateTimeFormat);

    public const string EnumFormat = nameof(EnumFormat);

    public const string GuidFormat = nameof(GuidFormat);

    public const string Json = nameof(Json);

    public const string NumericFormat = nameof(NumericFormat);

    public const string Regex = nameof(Regex);

    public const string TimeOnlyFormat = nameof(TimeOnlyFormat);

    public const string TimeSpanFormat = nameof(TimeSpanFormat);

    public const string Uri = nameof(Uri);

    public const string Xml = nameof(Xml);
}
#endif
