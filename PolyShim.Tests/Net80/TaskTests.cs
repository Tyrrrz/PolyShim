using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class TaskTests
{
    [Fact]
    public async Task ConfigureAwait_Options_Test()
    {
        // Arrange
        var task = Task.Delay(10);

        // Act & assert
        await task.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
    }

    [Fact]
    public async Task ConfigureAwait_Options_SuppressThrowing_Test()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act & assert
        // SuppressThrowing cannot be properly emulated on older target frameworks,
        // so it's treated as a no-op and the task can still be awaited normally.
        await task.ConfigureAwait(
            ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.SuppressThrowing
        );
    }

    [Fact]
    public async Task ConfigureAwait_Result_Options_Test()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);

        // Assert
        result.Should().Be(42);
    }
}
