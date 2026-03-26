using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class EnumTests
{
    [Fact]
    public void Parse_Test()
    {
        // Act & assert
        Enum.Parse<DayOfWeek>("Friday").Should().Be(DayOfWeek.Friday);
        Enum.Parse<DayOfWeek>("friday", true).Should().Be(DayOfWeek.Friday);
        new Action(() => Enum.Parse<DayOfWeek>("Moonday")).Should().Throw<ArgumentException>();
        new Action(() => Enum.Parse<DayOfWeek>("friday", false))
            .Should()
            .Throw<ArgumentException>();
    }
}
