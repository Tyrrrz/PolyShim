using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class AsyncDisposableTests
{
    private class AsyncResource : IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return default;
        }
    }

    [Fact]
    public async Task DisposeAsync_Test()
    {
        // Arrange
        var resource = new AsyncResource();

        // Act
        await resource.DisposeAsync();

        // Assert
        resource.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public async Task DisposeAsync_AwaitUsing_Test()
    {
        // Arrange
        var resource = new AsyncResource();

        // Act
        await using (resource) { }

        // Assert
        resource.IsDisposed.Should().BeTrue();
    }
}
