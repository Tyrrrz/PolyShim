using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class RandomTests
{
    [Fact]
    public void GetItems_Test()
    {
        // Arrange
        var random = new Random(0);
        var choices = new[] { 1, 2, 3, 4, 5 };

        for (var i = 0; i < 100; i++)
        {
            // Act
            var items = random.GetItems(choices, 3);

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
        var random = new Random(0);
        var originalItems = new[] { 1, 2, 3, 4, 5 };

        for (var i = 0; i < 100; i++)
        {
            // Act
            var items = (int[])originalItems.Clone();
            random.Shuffle(items);

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
