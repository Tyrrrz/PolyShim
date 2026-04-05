using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class DateTimeOffsetTests
{
    [Fact]
    public void UnixEpoch_Test()
    {
        // Act
        var result = DateTimeOffset.UnixEpoch;

        // Assert
        result.Year.Should().Be(1970);
        result.Month.Should().Be(1);
        result.Day.Should().Be(1);
        result.Hour.Should().Be(0);
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
        result.Offset.Should().Be(TimeSpan.Zero);
    }
}
