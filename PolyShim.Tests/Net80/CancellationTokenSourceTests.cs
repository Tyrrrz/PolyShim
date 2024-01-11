using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class CancellationTokenSourceTests
{
    [Fact]
    public async Task CancelAsync_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        await cts.CancelAsync();

        // Assert
        cts.IsCancellationRequested.Should().BeTrue();
    }
}
