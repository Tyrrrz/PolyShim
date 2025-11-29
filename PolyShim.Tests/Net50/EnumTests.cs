using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class EnumTests
{
    [Fact]
    public void GetNames_Test()
    {
        // Act & assert
        Enum.GetNames<DayOfWeek>()
            .Should()
            .Equal("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
    }
}
