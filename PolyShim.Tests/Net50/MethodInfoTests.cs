using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class MethodInfoTests
{
    private static int Add(int a, int b) => a + b;

    private class Multiplier(int factor)
    {
        public int Multiply(int value) => value * factor;
    }

    [Fact]
    public void CreateDelegate_Test()
    {
        // Arrange
        var method = typeof(MethodInfoTests).GetMethod(
            nameof(Add),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
        )!;

        // Act
        var del = method.CreateDelegate<Func<int, int, int>>();

        // Assert
        del(2, 3).Should().Be(5);
    }

    [Fact]
    public void CreateDelegate_WithTarget_Test()
    {
        // Arrange
        var instance = new Multiplier(2);
        var method = typeof(Multiplier).GetMethod(nameof(Multiplier.Multiply))!;

        // Act
        var del = method.CreateDelegate<Func<int, int>>(instance);

        // Assert
        del(3).Should().Be(6);
    }
}
