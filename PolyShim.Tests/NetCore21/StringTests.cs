using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class StringTests
{
    [Fact]
    public void Contains_Positive_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.Contains("B", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Contains_Negative_Test()
    {
        // Arrange
        const string str = "abc";

        // Act
        var result = str.Contains("D", StringComparison.OrdinalIgnoreCase);

        // Assert
        result.Should().BeFalse();
    }
}