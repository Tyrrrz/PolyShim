using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class StringTests
{
    [Fact]
    public void StartsWith_Positive_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.StartsWith('a');

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void StartsWith_Negative_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.StartsWith('b');

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EndsWith_Positive_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.EndsWith('c');

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EndsWith_Negative_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.EndsWith('b');

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Contains_Positive_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.Contains('b');

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Contains_Negative_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.Contains('d');

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Split_Test()
    {
        // Arrange
        const string str = "a b c";

        // Act
        var result = str.Split(' ');

        // Assert
        result.Should().Equal("a", "b", "c");
    }
}