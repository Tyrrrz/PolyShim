using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class TextWriterTests
{
    [Fact]
    public async Task FlushAsync_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync("Hello");

        // Act
        await writer.FlushAsync(CancellationToken.None);

        // Assert
        stream.ToArray().Should().StartWith("Hello"u8.ToArray());
    }

    [Fact]
    public async Task FlushAsync_Cancellation_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await writer.FlushAsync(cts.Token);
        });
    }
}
