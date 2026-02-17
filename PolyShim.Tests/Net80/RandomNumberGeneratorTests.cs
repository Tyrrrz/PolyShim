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
}
