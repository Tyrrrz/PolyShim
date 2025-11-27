using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class StringTests
{
    [Fact]
    public void StartsWith_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.StartsWith('a').Should().BeTrue();
        str.StartsWith('b').Should().BeFalse();
    }

    [Fact]
    public void EndsWith_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.EndsWith('c').Should().BeTrue();
        str.EndsWith('b').Should().BeFalse();
    }

    [Fact]
    public void Contains_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.Contains('b').Should().BeTrue();
        str.Contains('d').Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Test()
    {
        // Arrange
        const string str1 = "Hello";
        const string str2 = "Hello";
        const string str3 = "hello";

        // Act & assert
        str1.GetHashCode(StringComparison.Ordinal)
            .Should()
            .Be(str2.GetHashCode(StringComparison.Ordinal));
        str1.GetHashCode(StringComparison.OrdinalIgnoreCase)
            .Should()
            .Be(str3.GetHashCode(StringComparison.OrdinalIgnoreCase));
        str1.GetHashCode(StringComparison.Ordinal)
            .Should()
            .NotBe(str3.GetHashCode(StringComparison.Ordinal));
    }

    [Fact]
    public void Replace_StringComparison_Test()
    {
        // Arrange
        const string str = "abcFooABCbar";

        // Act & assert
        str.Replace("ab", "XY", StringComparison.Ordinal).Should().Be("XYcFooABCbar");
        str.Replace("ab", "XY", StringComparison.OrdinalIgnoreCase).Should().Be("XYcFooXYCbar");
        str.Replace("ab", null, StringComparison.OrdinalIgnoreCase).Should().Be("cFooCbar");
    }

    [Fact]
    public void Replace_CultureInfo_Test()
    {
        // Arrange
        const string str = "abcFooABCbar";

        // Act & assert
        str.Replace("ab", "XY", false, CultureInfo.InvariantCulture).Should().Be("XYcFooABCbar");
        str.Replace("ab", "XY", true, CultureInfo.InvariantCulture).Should().Be("XYcFooXYCbar");
        str.Replace("ab", null, true, CultureInfo.InvariantCulture).Should().Be("cFooCbar");
    }

    [Fact]
    public void Split_Char_Test()
    {
        // Arrange
        const string str = "a b c";

        // Act & assert
        str.Split(' ').Should().Equal("a", "b", "c");
        str.Split('b').Should().Equal("a ", " c");
    }

    [Fact]
    public void Split_Char_Count_Test()
    {
        // Arrange
        const string str = "a b c";

        // Act & assert
        str.Split(' ', 3).Should().Equal("a", "b", "c");
        str.Split(' ', 2).Should().Equal("a", "b c");
        str.Split(' ', 1).Should().Equal("a b c");
    }

    [Fact]
    public void Split_String_Test()
    {
        // Arrange
        const string str = "a b c";

        // Act & assert
        str.Split(" ").Should().Equal("a", "b", "c");
        str.Split("b").Should().Equal("a ", " c");
    }

    [Fact]
    public void Split_String_Count_Test()
    {
        // Arrange
        const string str = "a b c";

        // Act & assert
        str.Split(" ", 3).Should().Equal("a", "b", "c");
        str.Split(" ", 2).Should().Equal("a", "b c");
        str.Split(" ", 1).Should().Equal("a b c");
    }
}
