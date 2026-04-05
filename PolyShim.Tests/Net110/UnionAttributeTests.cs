using System;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class UnionAttributeTests
{
    [Fact]
    public void Ctor_Test()
    {
        // Act
        var attribute = new UnionAttribute();

        // Assert
        attribute.Should().BeAssignableTo<Attribute>();
    }
}
