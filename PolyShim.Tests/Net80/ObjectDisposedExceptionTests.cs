using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class ObjectDisposedExceptionTests
{
    [Fact]
    public void ThrowIf_Type_NotDisposed_Test()
    {
        // Act & assert (should not throw)
        ObjectDisposedException.ThrowIf(false, this.GetType());
    }

    [Fact]
    public void ThrowIf_Type_Disposed_Test()
    {
        // Act
        var act = () => ObjectDisposedException.ThrowIf(true, this.GetType());

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void ThrowIf_Object_NotDisposed_Test()
    {
        // Act & assert (should not throw)
        ObjectDisposedException.ThrowIf(false, this);
    }

    [Fact]
    public void ThrowIf_Object_Disposed_Test()
    {
        // Act
        var act = () => ObjectDisposedException.ThrowIf(true, this);

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }
}
