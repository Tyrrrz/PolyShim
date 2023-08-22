using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class CallerArgumentExpressionAttributeTests
{
    private static string? TestMethod(
        bool condition,
        [CallerArgumentExpression("condition")] string? expression = null
    ) => expression;

    [Fact]
    public void Initialization_Test()
    {
        // Act
        var result = TestMethod(1 + 1 == 4 - 2);

        // Assert
        result.Should().Be("1 + 1 == 4 - 2");
    }
}
