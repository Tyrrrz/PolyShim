using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class WeakReferenceTests
{
    [Fact]
    public void TryGetTarget_Test()
    {
        // Arrange
        var obj = new object();
        var reference = new WeakReference<object>(obj);

        // Act & assert
        reference.TryGetTarget(out var target).Should().BeTrue();
        target.Should().BeSameAs(obj);
    }

    [Fact]
    public void TryGetTarget_AfterSet_Test()
    {
        // Arrange
        var reference = new WeakReference<object>(new object());
        var obj = new object();
        reference.SetTarget(obj);

        // Act & assert
        reference.TryGetTarget(out var target).Should().BeTrue();
        target.Should().BeSameAs(obj);
    }
}
