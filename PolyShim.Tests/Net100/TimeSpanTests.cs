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
    }
}
