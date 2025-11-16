using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class MathTests
{
    [Fact]
    public void Clamp_Test()
    {
        // Act & assert
        Math.Clamp((nint)5, (nint)1, (nint)10).Should().Be((nint)5);
        Math.Clamp((nint)0, (nint)1, (nint)10).Should().Be((nint)1);
        Math.Clamp((nint)15, (nint)1, (nint)10).Should().Be((nint)10);
    }
}
