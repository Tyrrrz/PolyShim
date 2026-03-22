using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class IntPtrTests
{
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        IntPtr.TryParse("42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(new IntPtr(42));
    }
}
