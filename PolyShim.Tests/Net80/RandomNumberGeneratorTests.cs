using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void GetItems_Test()
    {
        // Arrange
        var choices = new[] { 1, 2, 3, 4, 5 };

        // Act
        for (var i = 0; i < 100; i++)
        {
            var items = RandomNumberGenerator.GetItems(choices, 3);

            // Assert
            items.Should().HaveCount(3);

            foreach (var item in items)
            {
                choices.Should().Contain(item);
            }
        }
    }

    [Fact]
    public void Shuffle_Test()
    {
        // Arrange
        var originalItems = new[] { 1, 2, 3, 4, 5 };

        // Act
        for (var i = 0; i < 100; i++)
        {
            var items = (int[])originalItems.Clone();
            RandomNumberGenerator.Shuffle(items);

            // Assert
            items.Should().HaveCount(originalItems.Length);
            items.Should().BeEquivalentTo(originalItems);

            foreach (var item in items)
            {
                originalItems.Should().Contain(item);
            }
        }
    }

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
