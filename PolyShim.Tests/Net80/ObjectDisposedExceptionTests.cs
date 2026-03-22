using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class ObjectDisposedExceptionTests
{
    [Fact]
    public void ThrowIf_Object_NotDisposed_Test()
    {
        // Arrange
        var obj = new object();

        // Act & assert (should not throw)
        ObjectDisposedException.ThrowIf(false, obj);
    }

    [Fact]
    public void ThrowIf_Object_Disposed_Test()
    {
        // Arrange
        var obj = new object();

        // Act
        var act = () => ObjectDisposedException.ThrowIf(true, obj);

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void ThrowIf_Type_NotDisposed_Test()
    {
        // Arrange
        var type = typeof(string);

        // Act & assert (should not throw)
        ObjectDisposedException.ThrowIf(false, type);
    }

    [Fact]
    public void ThrowIf_Type_Disposed_Test()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var act = () => ObjectDisposedException.ThrowIf(true, type);

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }
}
