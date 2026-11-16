using System;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class MathTests
{
    [Fact]
    public void Clamp_Test()
    {
        // Act & assert
        Math.Clamp(5, 1, (nint)10).Should().Be(5);
        Math.Clamp(0, 1, (nint)10).Should().Be(1);
        Math.Clamp(15, 1, (nint)10).Should().Be(10);
    }
}
