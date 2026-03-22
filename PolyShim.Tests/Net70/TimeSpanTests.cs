using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class TimeSpanTests
{
    [Fact]
    public void FromMicroseconds_Test()
    {
        // Act & assert
        TimeSpan.FromMicroseconds(1_000_000).Should().Be(TimeSpan.FromSeconds(1));
        TimeSpan.FromMicroseconds(500).Should().Be(TimeSpan.FromMilliseconds(0.5));
        TimeSpan.FromMicroseconds(0).Should().Be(TimeSpan.Zero);
    }
}
