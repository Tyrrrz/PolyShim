using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class RuntimeHelpersTests
{
    [Fact]
    public void GetSubArray_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        // Implicitly calls GetSubArray(...)
        array[2..^1].Should().BeEquivalentTo(new[] { 3, 4 });
    }
}