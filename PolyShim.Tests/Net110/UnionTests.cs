using System;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

// Union type with two variants, demonstrating the polyfills
record CircleShape(double Radius);

record RectangleShape(double Width, double Height);

union Shape(CircleShape, RectangleShape);

public class UnionTests
{
    [Fact]
    public void Shape_Circle_Switch_Test()
    {
        // Arrange
        Shape shape = new CircleShape(5.0);

        // Act
        var description = shape switch
        {
            CircleShape c => $"Circle with radius {c.Radius}",
            RectangleShape r => $"Rectangle {r.Width}x{r.Height}",
            _ => throw new InvalidOperationException("Unexpected shape")
        };

        // Assert
        description.Should().Be("Circle with radius 5");
    }

    [Fact]
    public void Shape_Rectangle_Switch_Test()
    {
        // Arrange
        Shape shape = new RectangleShape(3.0, 4.0);

        // Act
        var description = shape switch
        {
            CircleShape c => $"Circle with radius {c.Radius}",
            RectangleShape r => $"Rectangle {r.Width}x{r.Height}",
            _ => throw new InvalidOperationException("Unexpected shape")
        };

        // Assert
        description.Should().Be("Rectangle 3x4");
    }

    [Fact]
    public void Shape_IUnion_Value_Test()
    {
        // Arrange
        Shape shape = new CircleShape(5.0);

        // Act
        var iUnion = (IUnion)shape;

        // Assert
        iUnion.Value.Should().BeOfType<CircleShape>();
        iUnion.Value.Should().Be(new CircleShape(5.0));
    }
}
