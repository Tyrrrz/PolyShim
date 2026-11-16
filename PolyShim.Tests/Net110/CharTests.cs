using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class CharTests
{
    [Fact]
    public void Equals_Test()
    {
        // Act & assert
        'a'.Equals('a', StringComparison.Ordinal).Should().BeTrue();
        'a'.Equals('A', StringComparison.Ordinal).Should().BeFalse();
        'a'.Equals('A', StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        'a'.Equals('b', StringComparison.OrdinalIgnoreCase).Should().BeFalse();
    }
}
