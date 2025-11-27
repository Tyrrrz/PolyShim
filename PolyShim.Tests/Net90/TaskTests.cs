using System;
using System.Collections.Generic;
using System.Threading;
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

        // Act
        var iterationTask = Task.Run(async () =>
        {
            var tasks = new List<Task>();
            await foreach (var completedTask in Task.WhenEach(tcs1.Task, tcs2.Task, tcs3.Task))
            {
                tasks.Add(completedTask);
            }

            return tasks;
        });

        tcs2.SetResult();
        await Task.Delay(10);
        tcs1.SetResult();
        await Task.Delay(10);
        tcs3.SetResult();

        var completedTasks = await iterationTask;

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

        // Act
        var iterationTask = Task.Run(async () =>
        {
            var tasks = new List<Task>();
            await foreach (var completedTask in Task.WhenEach(tcs1.Task, tcs2.Task, tcs3.Task))
            {
                tasks.Add(completedTask);
            }

            return tasks;
        });

        tcs2.SetException(new InvalidOperationException("Test exception"));
        await Task.Delay(10);
        tcs1.SetResult();
        await Task.Delay(10);
        tcs3.SetResult();

        var completedTasks = await iterationTask;

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await completedTasks[0]);
        await completedTasks[1];
        await completedTasks[2];
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_AlreadyPartiallyCompleted_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource();
        var tcs2 = new TaskCompletionSource();
        var tcs3 = new TaskCompletionSource();

        tcs2.SetResult();

        // Act
        var iterationTask = Task.Run(async () =>
        {
            var tasks = new List<Task>();
            await foreach (var completedTask in Task.WhenEach(tcs1.Task, tcs2.Task, tcs3.Task))
            {
                tasks.Add(completedTask);
            }

            return tasks;
        });

        await Task.Delay(10);
        tcs1.SetResult();
        await Task.Delay(10);
        tcs3.SetResult();

        var completedTasks = await iterationTask;

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Result_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource<int>();
        var tcs2 = new TaskCompletionSource<int>();
        var tcs3 = new TaskCompletionSource<int>();

        // Act
        var iterationTask = Task.Run(async () =>
        {
            var tasks = new List<Task<int>>();

            // For some reason, dropping the array literal leads to call ambiguity on .NET Framework
            // ReSharper disable RedundantExplicitParamsArrayCreation
            await foreach (
                var completedTask in Task.WhenEach(new[] { tcs1.Task, tcs2.Task, tcs3.Task })
            )
            {
                tasks.Add(completedTask);
            }
            // ReSharper restore RedundantExplicitParamsArrayCreation

            return tasks;
        });

        tcs2.SetResult(11);
        await Task.Delay(10);
        tcs1.SetResult(22);
        await Task.Delay(10);
        tcs3.SetResult(33);

        var completedTasks = await iterationTask;

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

        // Act
        var iterationTask = Task.Run(async () =>
        {
            var tasks = new List<Task<int>>();

            // For some reason, dropping the array literal leads to call ambiguity on .NET Framework
            // ReSharper disable RedundantExplicitParamsArrayCreation
            await foreach (
                var completedTask in Task.WhenEach(new[] { tcs1.Task, tcs2.Task, tcs3.Task })
            )
            {
                tasks.Add(completedTask);
            }
            // ReSharper restore RedundantExplicitParamsArrayCreation

            return tasks;
        });

        tcs2.SetException(new InvalidOperationException("Test exception"));
        await Task.Delay(10);
        tcs1.SetResult(22);
        await Task.Delay(10);
        tcs3.SetResult(33);

        var completedTasks = await iterationTask;

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await completedTasks[0]);
        (await completedTasks[1]).Should().Be(22);
        (await completedTasks[2]).Should().Be(33);
    }

    [Fact(Timeout = 5000)]
    public async Task WhenEach_Result_AlreadyPartiallyCompleted_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource<int>();
        var tcs2 = new TaskCompletionSource<int>();
        var tcs3 = new TaskCompletionSource<int>();

        tcs2.SetResult(11);

        // Act
        var iterationTask = Task.Run(async () =>
        {
            var tasks = new List<Task<int>>();

            // For some reason, dropping the array literal leads to call ambiguity on .NET Framework
            // ReSharper disable RedundantExplicitParamsArrayCreation
            await foreach (
                var completedTask in Task.WhenEach(new[] { tcs1.Task, tcs2.Task, tcs3.Task })
            )
            {
                tasks.Add(completedTask);
            }
            // ReSharper restore RedundantExplicitParamsArrayCreation

            return tasks;
        });

        await Task.Delay(10);
        tcs1.SetResult(22);
        await Task.Delay(10);
        tcs3.SetResult(33);

        var completedTasks = await iterationTask;

        // Assert
        completedTasks.Should().Equal(tcs2.Task, tcs1.Task, tcs3.Task);
        (await completedTasks[0]).Should().Be(11);
        (await completedTasks[1]).Should().Be(22);
        (await completedTasks[2]).Should().Be(33);
    }
}
