using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class MathTests
{
    [Fact]
    public void Clamp_Test()
    {
        // Act & assert
        Math.Clamp(5, 1, 10).Should().Be(5);
        Math.Clamp(0, 1, 10).Should().Be(1);
        Math.Clamp(15, 1, 10).Should().Be(10);
        Math.Clamp(5.5, 1.0, 10.0).Should().Be(5.5);
        Math.Clamp(0.5, 1.0, 10.0).Should().Be(1.0);
        Math.Clamp(15.5, 1.0, 10.0).Should().Be(10.0);
    }
}
