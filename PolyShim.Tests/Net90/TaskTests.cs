using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class TaskTests
{
    [Fact(Timeout = 5000)]
    public async Task WhenEach_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource();
        var tcs2 = new TaskCompletionSource();
        var tcs3 = new TaskCompletionSource();

        var completedTasks = new List<Task>();

        void CompleteNext()
        {
            // Complete tasks in a specific order
            if (completedTasks.Count == 0)
                tcs2.SetResult();
            else if (completedTasks.Count == 1)
                tcs1.SetResult();
            else if (completedTasks.Count == 2)
                tcs3.SetResult();
        }

        // Act
        CompleteNext();
        await foreach (var completedTask in Task.WhenEach(tcs1.Task, tcs2.Task, tcs3.Task))
        {
            completedTasks.Add(completedTask);
            CompleteNext();
        }

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Empty_Test()
    {
        // Act
        var tasks = new List<Task>();
        await foreach (var completedTask in Task.WhenEach())
        {
            tasks.Add(completedTask);
        }

        // Assert
        tasks.Should().BeEmpty();
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Exception_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource();
        var tcs2 = new TaskCompletionSource();
        var tcs3 = new TaskCompletionSource();

        var completedTasks = new List<Task>();

        void CompleteNext()
        {
            // Complete tasks in a specific order
            if (completedTasks.Count == 0)
                tcs2.SetException(new InvalidOperationException("Test exception"));
            else if (completedTasks.Count == 1)
                tcs1.SetResult();
            else if (completedTasks.Count == 2)
                tcs3.SetResult();
        }

        // Act
        CompleteNext();
        await foreach (var completedTask in Task.WhenEach(tcs1.Task, tcs2.Task, tcs3.Task))
        {
            completedTasks.Add(completedTask);
            CompleteNext();
        }

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await completedTasks[0]);
        await completedTasks[1];
        await completedTasks[2];
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Result_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource<int>();
        var tcs2 = new TaskCompletionSource<int>();
        var tcs3 = new TaskCompletionSource<int>();

        var completedTasks = new List<Task<int>>();

        void CompleteNext()
        {
            // Complete tasks in a specific order
            if (completedTasks.Count == 0)
                tcs2.SetResult(11);
            else if (completedTasks.Count == 1)
                tcs1.SetResult(22);
            else if (completedTasks.Count == 2)
                tcs3.SetResult(33);
        }

        // Act
        CompleteNext();
        await foreach (
            var completedTask in Task.WhenEach(new[] { tcs1.Task, tcs2.Task, tcs3.Task })
        )
        {
            completedTasks.Add(completedTask);
            CompleteNext();
        }

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
        (await completedTasks[0]).Should().Be(11);
        (await completedTasks[1]).Should().Be(22);
        (await completedTasks[2]).Should().Be(33);
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Result_Empty_Test()
    {
        // Act
        var tasks = new List<Task<int>>();
        await foreach (var completedTask in Task.WhenEach<int>())
        {
            tasks.Add(completedTask);
        }

        // Assert
        tasks.Should().BeEmpty();
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Result_Exception_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource<int>();
        var tcs2 = new TaskCompletionSource<int>();
        var tcs3 = new TaskCompletionSource<int>();

        var completedTasks = new List<Task<int>>();

        void CompleteNext()
        {
            // Complete tasks in a specific order
            if (completedTasks.Count == 0)
                tcs2.SetException(new InvalidOperationException("Test exception"));
            else if (completedTasks.Count == 1)
                tcs1.SetResult(22);
            else if (completedTasks.Count == 2)
                tcs3.SetResult(33);
        }

        // Act
        CompleteNext();
        await foreach (
            var completedTask in Task.WhenEach(new[] { tcs1.Task, tcs2.Task, tcs3.Task })
        )
        {
            completedTasks.Add(completedTask);
            CompleteNext();
        }

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await completedTasks[0]);
        (await completedTasks[1]).Should().Be(22);
        (await completedTasks[2]).Should().Be(33);
    }
}
