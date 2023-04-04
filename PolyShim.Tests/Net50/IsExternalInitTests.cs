using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class IsExternalInitTests
{
    private record MyRecord(string Foo, int Bar);

    [Fact]
    public void Initialization_Test()
    {
        // Act
        var record = new MyRecord("hello world", 42);

        // Assert
        record.Foo.Should().Be("hello world");
        record.Bar.Should().Be(42);
    }
}