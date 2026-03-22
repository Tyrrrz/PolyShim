using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SingleTests
{
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        float.TryParse("3.14".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Should().BeApproximately(3.14f, 0.001f);

        float.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }
}
