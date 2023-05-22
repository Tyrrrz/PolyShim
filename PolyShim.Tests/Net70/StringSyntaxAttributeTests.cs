using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class StringSyntaxAttributeTests
{
    // The parameter should be highlighted at the call site
    private static Regex CreateRegex(
        [StringSyntax(StringSyntaxAttribute.Regex)]
        string pattern
    ) => new(pattern);

    [Fact]
    public void Initialization_Test()
    {
        // Act
        var regex = CreateRegex(@"foo\sbar");

        // Assert
        regex.IsMatch("foo bar").Should().BeTrue();
    }
}