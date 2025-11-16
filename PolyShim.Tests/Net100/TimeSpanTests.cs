using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class TimeSpanTests
{
    [Fact]
    public void FromMilliseconds_Test()
    {
        // Act & assert
        TimeSpan.FromMilliseconds(1234).Should().Be(TimeSpan.FromSeconds(1.234));
        TimeSpan.FromMilliseconds(0).Should().Be(TimeSpan.Zero);
        TimeSpan.FromMilliseconds(-1000).Should().Be(TimeSpan.FromSeconds(-1));
    }
}
