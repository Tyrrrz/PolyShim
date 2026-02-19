using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class TimerTests
{
    [Fact]
    public async Task Timer_FiresCallback_AfterDueTime_Test()
    {
        // Arrange
        var fired = false;

        // Act
        using var timer = new Timer(
            _ => fired = true,
            null,
            TimeSpan.FromMilliseconds(50),
            Timeout.InfiniteTimeSpan
        );
        await Task.Delay(TimeSpan.FromMilliseconds(500));

        // Assert
        fired.Should().BeTrue();
    }

    [Fact]
    public async Task Timer_FiresCallbackRepeatedly_WithPeriod_Test()
    {
        // Arrange
        var count = 0;

        // Act
        using var timer = new Timer(
            _ => Interlocked.Increment(ref count),
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(50)
        );
        await Task.Delay(TimeSpan.FromMilliseconds(500));

        // Assert
        count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task Timer_DoesNotFire_WhenDueTimeIsInfinite_Test()
    {
        // Arrange
        var fired = false;

        // Act
        using var timer = new Timer(
            _ => fired = true,
            null,
            Timeout.InfiniteTimeSpan,
            Timeout.InfiniteTimeSpan
        );
        await Task.Delay(TimeSpan.FromMilliseconds(200));

        // Assert
        fired.Should().BeFalse();
    }

    [Fact]
    public async Task Timer_Change_UpdatesDueTime_Test()
    {
        // Arrange
        var fired = false;
        using var timer = new Timer(
            _ => fired = true,
            null,
            Timeout.InfiniteTimeSpan,
            Timeout.InfiniteTimeSpan
        );

        // Act
        timer.Change(TimeSpan.FromMilliseconds(50), Timeout.InfiniteTimeSpan);
        await Task.Delay(TimeSpan.FromMilliseconds(500));

        // Assert
        fired.Should().BeTrue();
    }

    [Fact]
    public async Task Timer_Dispose_StopsFiring_Test()
    {
        // Arrange
        var count = 0;
        var timer = new Timer(
            _ => Interlocked.Increment(ref count),
            null,
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(100)
        );

        // Act
        timer.Dispose();
        await Task.Delay(TimeSpan.FromMilliseconds(400));

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void Timer_Change_ReturnsFalse_AfterDispose_Test()
    {
        // Arrange
        var timer = new Timer(_ => { }, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        timer.Dispose();

        // Act
        var result = timer.Change(TimeSpan.FromMilliseconds(50), Timeout.InfiniteTimeSpan);

        // Assert
        result.Should().BeFalse();
    }
}
