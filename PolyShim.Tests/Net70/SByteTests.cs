using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SByteTests
{
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        sbyte.TryParse("-42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(-42);

        sbyte.TryParse("999".AsSpan(), null, out _).Should().BeFalse();
    }
}
