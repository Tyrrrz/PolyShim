using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class TimeSpanTests
{
    [Fact]
    public void FromMilliseconds_Test()
    {
        // Act & assert
        TimeSpan.FromMilliseconds(1234, 567).Should().Be(TimeSpan.FromSeconds(1.234567));
    }
}
