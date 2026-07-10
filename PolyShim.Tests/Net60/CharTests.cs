using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class CharTests
{
    [Fact]
    public void IsAscii_Test()
    {
        // Act & assert
        char.IsAscii('A').Should().BeTrue();
        char.IsAscii('z').Should().BeTrue();
        char.IsAscii('\x7f').Should().BeTrue();
        char.IsAscii('\x80').Should().BeFalse();
        char.IsAscii('é').Should().BeFalse();
    }
}
