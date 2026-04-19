using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class WeakReferenceTests
{
    [Fact]
    public void TryGetTarget_WhenAlive_Test()
    {
        // Arrange
        var obj = new object();
        var reference = new WeakReference<object>(obj);

        // Act & Assert
        reference.TryGetTarget(out var target).Should().BeTrue();
        target.Should().BeSameAs(obj);
    }

    [Fact]
    public void TryGetTarget_AfterSetTarget_Test()
    {
        // Arrange
        var reference = new WeakReference<object>(new object());
        var obj = new object();
        reference.SetTarget(obj);

        // Act & Assert
        reference.TryGetTarget(out var target).Should().BeTrue();
        target.Should().BeSameAs(obj);
    }
}
