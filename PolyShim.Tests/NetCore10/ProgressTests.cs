using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class ProgressTests
{
    [Fact]
    public void Report_WithHandler_Test()
    {
        // Arrange
        using var received = new ManualResetEventSlim();
        var receivedValue = default(int);
        var progress = new Progress<int>(v =>
        {
            receivedValue = v;
            received.Set();
        });

        // Act
        ((IProgress<int>)progress).Report(42);

        // Assert
        received.Wait(TimeSpan.FromSeconds(5)).Should().BeTrue();
        receivedValue.Should().Be(42);
    }

    [Fact]
    public void Report_WithEvent_Test()
    {
        // Arrange
        using var received = new ManualResetEventSlim();
        var receivedValue = default(int);
        var progress = new Progress<int>();
        progress.ProgressChanged += (_, v) =>
        {
            receivedValue = v;
            received.Set();
        };

        // Act
        ((IProgress<int>)progress).Report(99);

        // Assert
        received.Wait(TimeSpan.FromSeconds(5)).Should().BeTrue();
        receivedValue.Should().Be(99);
    }

    [Fact]
    public void Report_NoHandler_Test()
    {
        // Arrange
        var progress = new Progress<int>();

        // Act & assert
        var act = () => ((IProgress<int>)progress).Report(1);
        act.Should().NotThrow();
    }
}
