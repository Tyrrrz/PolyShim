using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class RangeTests
{
    [Fact]
    public void Slice_StartAndEnd_Test()
    {
        // Arrange
        const string str = "Hello world";

        // Act
        var result = str[6..11];

        // Assert
        result.Should().Be("world");
    }

    [Fact]
    public void Slice_StartOnly_Test()
    {
        // Arrange
        const string str = "Hello world";

        // Act
        var result = str[..5];

        // Assert
        result.Should().Be("Hello");
    }

    [Fact]
    public void Slice_EndOnly_Test()
    {
        // Arrange
        const string str = "Hello world";

        // Act
        var result = str[6..];

        // Assert
        result.Should().Be("world");
    }
}