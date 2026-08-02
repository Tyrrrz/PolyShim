using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class MathTests
{
    [Fact]
    public void Log2_Test()
    {
        // Act & assert
        Math.Log2(8.0).Should().Be(3.0);
        Math.Log2(1.0).Should().Be(0.0);
        Math.Log2(0.0).Should().Be(double.NegativeInfinity);
        Math.Log2(double.PositiveInfinity).Should().Be(double.PositiveInfinity);
        Math.Log2(-1.0).Should().Be(double.NaN);
        Math.Log2(double.NaN).Should().Be(double.NaN);
    }
}
