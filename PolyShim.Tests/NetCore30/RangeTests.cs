using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class RangeTests
{
    [Fact]
    public void Slice_Test()
    {
        // Arrange
        const string str = "Hello world!";

        // Act & assert
        str[6..11].Should().Be("world");
        str[..5].Should().Be("Hello");
        str[6..].Should().Be("world!");
    }
}