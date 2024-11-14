using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class OverloadResolutionPriorityAttributeTests
{
    [OverloadResolutionPriority(1)]
    private int Foo(int value = 0) => value;

    private int Foo() => 42;

    [Fact]
    public void Initialization_Test()
    {
        // Act & assert
        Foo().Should().Be(0);
    }
}
