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
        var act1 = () => Enum.Parse<DayOfWeek>("Moonday");
        act1.Should().Throw<ArgumentException>();
        var act2 = () => Enum.Parse<DayOfWeek>("friday", false);
        act2.Should().Throw<ArgumentException>();
    }
}
