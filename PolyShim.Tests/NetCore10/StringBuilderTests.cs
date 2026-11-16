using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class StringBuilderTests
{
    [Fact]
    public void Clear_Test()
    {
        // Arrange
        var builder = new StringBuilder("hello");

        // Act
        var result = builder.Clear();

        // Assert
        result.Should().BeSameAs(builder);
        builder.Length.Should().Be(0);
        builder.ToString().Should().BeEmpty();

        // The builder can be reused afterwards
        builder.Append("world").ToString().Should().Be("world");
    }
}
