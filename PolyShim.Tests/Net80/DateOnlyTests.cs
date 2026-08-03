using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class DateOnlyTests
{
    [Fact]
    public void Deconstruct_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act
        var (year, month, day) = date;

        // Assert
        year.Should().Be(2023);
        month.Should().Be(5);
        day.Should().Be(17);
    }
}
