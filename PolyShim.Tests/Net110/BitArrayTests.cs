using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class BitArrayTests
{
    [Fact]
    public void PopCount_Test()
    {
        // Arrange
        var bits = new BitArray(new[] { true, false, true, true });

        // Act & assert
        bits.PopCount().Should().Be(3);
        new BitArray(0).PopCount().Should().Be(0);
    }
}
