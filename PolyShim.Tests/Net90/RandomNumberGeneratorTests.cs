using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void GetHexString_Test()
    {
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            RandomNumberGenerator.GetHexString(16).Should().MatchRegex("^[0-9A-F]{16}$");
            RandomNumberGenerator.GetHexString(16, true).Should().MatchRegex("^[0-9a-f]{16}$");
            RandomNumberGenerator.GetHexString(15).Should().MatchRegex("^[0-9A-F]{15}$");
        }
    }

    [Fact]
    public void GetString_Array_Test()
    {
        // Arrange
        var choices =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        // Act
        for (var i = 0; i < 100; i++)
        {
            var str = RandomNumberGenerator.GetString(choices, 16);

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
        var choices = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".AsSpan();

        // Act
        for (var i = 0; i < 100; i++)
        {
            var str = RandomNumberGenerator.GetString(choices, 16);

            // Assert
            str.Length.Should().Be(16);

            foreach (var ch in str)
            {
                choices.ToArray().Should().Contain(ch);
            }
        }
    }
}
