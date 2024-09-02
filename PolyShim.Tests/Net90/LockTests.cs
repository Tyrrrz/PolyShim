using System.Threading;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class LockTests
{
    [Fact]
    public void Enter_Test()
    {
        // Arrange
        var syncRoot = new Lock();

        // Act & assert
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
        syncRoot.Enter();
        syncRoot.IsHeldByCurrentThread.Should().BeTrue();
    }

    [Fact]
    public void TryEnter_Test()
    {
        // Arrange
        var syncRoot = new Lock();

        // Act & assert
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
        syncRoot.TryEnter().Should().BeTrue();
        syncRoot.IsHeldByCurrentThread.Should().BeTrue();
    }

    [Fact]
    public void TryEnter_Timeout_Test()
    {
        // Arrange
        var syncRoot = new Lock();

        // Act & assert
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
        syncRoot.TryEnter(100).Should().BeTrue();
        syncRoot.IsHeldByCurrentThread.Should().BeTrue();
    }

    [Fact]
    public void Exit_Test()
    {
        // Arrange
        var syncRoot = new Lock();

        // Act & assert
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
        syncRoot.Enter();
        syncRoot.IsHeldByCurrentThread.Should().BeTrue();
        syncRoot.Exit();
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
    }

    [Fact]
    public void EnterScope_Test()
    {
        // Arrange
        var syncRoot = new Lock();

        // Act & assert
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
        using (syncRoot.EnterScope())
        {
            syncRoot.IsHeldByCurrentThread.Should().BeTrue();
        }
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
    }

    [Fact]
    public void EnterScope_Layered_Test()
    {
        // Arrange
        var syncRoot = new Lock();

        // Act & assert
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
        using (syncRoot.EnterScope())
        {
            syncRoot.IsHeldByCurrentThread.Should().BeTrue();
            using (syncRoot.EnterScope())
            {
                syncRoot.IsHeldByCurrentThread.Should().BeTrue();
            }
            syncRoot.IsHeldByCurrentThread.Should().BeTrue();
        }
        syncRoot.IsHeldByCurrentThread.Should().BeFalse();
    }
}
