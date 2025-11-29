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
        Assert.Throws<ArgumentException>(() => Enum.Parse<DayOfWeek>("Moonday"));
        Assert.Throws<ArgumentException>(() => Enum.Parse<DayOfWeek>("friday", false));
    }
}
