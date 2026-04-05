using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

file record MyRecord(string Foo, int Bar);

public class IsExternalInitTests
{
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
