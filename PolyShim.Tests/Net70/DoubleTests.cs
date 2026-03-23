using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class DoubleTests
{
#if !NETFRAMEWORK
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        double.TryParse("3.14159".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Should().BeApproximately(3.14159, 0.00001);

        double.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
