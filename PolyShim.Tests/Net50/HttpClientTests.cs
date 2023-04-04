using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

// These tests cover HttpContent polyfills as well
public class HttpClientTests
{
    [Fact]
    public async Task GetStreamAsync_Positive_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        using var httpClient = new HttpClient();

        // Act
        using var stream = await httpClient.GetStreamAsync("https://example.com", cancellationToken);
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        // Assert
        content.Should().Contain("Example Domain");
    }

    [Fact]
    public async Task GetStreamAsync_Negative_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        using var httpClient = new HttpClient();

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await httpClient.GetStreamAsync("https://example.com", cancellationToken)
        );

        // Assert
        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task GetByteArrayAsync_Positive_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        using var httpClient = new HttpClient();

        // Act
        var bytes = await httpClient.GetByteArrayAsync("https://example.com", cancellationToken);
        var content = Encoding.UTF8.GetString(bytes);

        // Assert
        content.Should().Contain("Example Domain");
    }

    [Fact]
    public async Task GetByteArrayAsync_Negative_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        using var httpClient = new HttpClient();

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await httpClient.GetByteArrayAsync("https://example.com", cancellationToken)
        );

        // Assert
        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task GetStringAsync_Positive_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        using var httpClient = new HttpClient();

        // Act
        var content = await httpClient.GetStringAsync("https://example.com", cancellationToken);

        // Assert
        content.Should().Contain("Example Domain");
    }

    [Fact]
    public async Task GetStringAsync_Negative_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        using var httpClient = new HttpClient();

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await httpClient.GetStringAsync("https://example.com", cancellationToken)
        );

        // Assert
        ex.CancellationToken.Should().Be(cancellationToken);
    }
}