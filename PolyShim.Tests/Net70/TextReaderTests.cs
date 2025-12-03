using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class TextReaderTests
{
    [Fact]
    public async Task ReadLineAsync_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Line 1\nLine 2\nLine 3\n"u8.ToArray());
        using var reader = new StreamReader(stream);

        // Act
        var line1 = await reader.ReadLineAsync(CancellationToken.None);
        var line2 = await reader.ReadLineAsync(CancellationToken.None);
        var line3 = await reader.ReadLineAsync(CancellationToken.None);
        var line4 = await reader.ReadLineAsync(CancellationToken.None);

        // Assert
        line1.Should().Be("Line 1");
        line2.Should().Be("Line 2");
        line3.Should().Be("Line 3");
        line4.Should().BeNull();
    }

    [Fact]
    public async Task ReadLineAsync_Cancellation_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Line 1\nLine 2\nLine 3\n"u8.ToArray());
        using var reader = new StreamReader(stream);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await reader.ReadLineAsync(cts.Token);
        });
    }

    [Fact]
    public async Task ReadToEndAsync_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello, World!"u8.ToArray());
        using var reader = new StreamReader(stream);

        // Act
        var content = await reader.ReadToEndAsync(CancellationToken.None);

        // Assert
        content.Should().Be("Hello, World!");
    }

    [Fact]
    public async Task ReadToEndAsync_Cancellation_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello, World!"u8.ToArray());
        using var reader = new StreamReader(stream);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await reader.ReadToEndAsync(cts.Token);
        });
    }
}
