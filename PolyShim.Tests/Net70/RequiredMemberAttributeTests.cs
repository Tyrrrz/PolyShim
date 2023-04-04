using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class RequiredMemberAttributeTests
{
    private class MyClass
    {
        public required string Foo { get; init; }

        public required int Bar { get; init; }
    }

    [Fact]
    public void Initialization_Test()
    {
        // Act
        var myClass = new MyClass
        {
            Foo = "hello world",
            Bar = 42
        };

        // Assert
        myClass.Foo.Should().Be("hello world");
        myClass.Bar.Should().Be(42);
    }
}