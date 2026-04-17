using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class ProgressTests
{
    [Fact]
    public void Report_WithHandler_InvokesHandler()
    {
        // Arrange
        var received = new ManualResetEventSlim();
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

    // ProgressChanged uses EventHandler<T> which requires T : EventArgs on .NET 3.5/4.0,
    // so this test is only compiled on platforms where the event is available.
#if !NETFRAMEWORK || NET45_OR_GREATER
    [Fact]
    public void Report_WithEvent_FiresEvent()
    {
        // Arrange
        var received = new ManualResetEventSlim();
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
#endif

    [Fact]
    public void Report_NoHandlerOrEvent_DoesNotThrow()
    {
        // Arrange
        var progress = new Progress<int>();

        // Act & assert
        var act = () => ((IProgress<int>)progress).Report(1);
        act.Should().NotThrow();
    }
}
