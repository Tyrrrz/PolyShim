using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class RandomTests
{
    [Fact]
    public void GetHexString_Test()
    {
        // Arrange
        var random = new Random(0);

        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            random.GetHexString(16).Should().MatchRegex("^[0-9A-F]{16}$");
            random.GetHexString(16, true).Should().MatchRegex("^[0-9a-f]{16}$");
        }
    }

    [Fact]
    public void GetString_Array_Test()
    {
        // Arrange
        var random = new Random(0);
        var choices =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        // Act
        for (var i = 0; i < 100; i++)
        {
            var str = random.GetString(choices, 16);

            // Assert
            str.Length.Should().Be(16);

            foreach (var ch in str)
            {
                choices.Should().Contain(ch);
            }
        }
    }

    [Fact]
    public void GetString_Span_Test()
    {
        // Arrange
        var random = new Random(0);
        var choices = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".AsSpan();

        // Act
        for (var i = 0; i < 100; i++)
        {
            var str = random.GetString(choices, 16);

            // Assert
            str.Length.Should().Be(16);

            foreach (var ch in str)
            {
                choices.ToArray().Should().Contain(ch);
            }
        }
    }
}
