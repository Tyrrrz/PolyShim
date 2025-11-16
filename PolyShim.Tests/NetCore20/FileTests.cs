using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class FileTests
{
    [Fact]
    public async Task AppendAllLinesAsync_Test()
    {
        // Arrange
        var linesToAppend = new[] { "Line 1", "Line 2", "Line 3" };
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Act
            await File.AppendAllLinesAsync(tempFilePath, linesToAppend);

            // Assert
            var readLines = await File.ReadAllLinesAsync(tempFilePath);
            readLines.Should().Equal(linesToAppend);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task AppendAllTextAsync_Test()
    {
        // Arrange
        var textToAppend = "Hello, World!" + Environment.NewLine + "This is a test.";
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Act
            await File.AppendAllTextAsync(tempFilePath, textToAppend);

            // Assert
            var readText = await File.ReadAllTextAsync(tempFilePath);
            readText.Should().Be(textToAppend);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task ReadAllBytesAsync_Test()
    {
        // Arrange
        var bytesToWrite = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var tempFilePath = Path.GetTempFileName();

        try
        {
            await File.WriteAllBytesAsync(tempFilePath, bytesToWrite);

            // Act
            var readBytes = await File.ReadAllBytesAsync(tempFilePath);

            // Assert
            readBytes.Should().Equal(bytesToWrite);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task ReadAllLinesAsync_Test()
    {
        // Arrange
        var linesToWrite = new[] { "First line", "Second line", "Third line" };
        var tempFilePath = Path.GetTempFileName();

        try
        {
            await File.WriteAllLinesAsync(tempFilePath, linesToWrite);

            // Act
            var readLines = await File.ReadAllLinesAsync(tempFilePath);

            // Assert
            readLines.Should().Equal(linesToWrite);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task ReadAllTextAsync_Test()
    {
        // Arrange
        var textToWrite = "This is a sample text for testing ReadAllTextAsync.";
        var tempFilePath = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(tempFilePath, textToWrite);

            // Act
            var readText = await File.ReadAllTextAsync(tempFilePath);

            // Assert
            readText.Should().Be(textToWrite);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task WriteAllBytesAsync_Test()
    {
        // Arrange
        var bytesToWrite = new byte[] { 10, 20, 30, 40, 50 };
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Act
            await File.WriteAllBytesAsync(tempFilePath, bytesToWrite);

            // Assert
            var readBytes = await File.ReadAllBytesAsync(tempFilePath);
            readBytes.Should().Equal(bytesToWrite);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task WriteAllLinesAsync_Test()
    {
        // Arrange
        var linesToWrite = new[] { "Alpha", "Beta", "Gamma" };
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Act
            await File.WriteAllLinesAsync(tempFilePath, linesToWrite);

            // Assert
            var readLines = await File.ReadAllLinesAsync(tempFilePath);
            readLines.Should().Equal(linesToWrite);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public async Task WriteAllTextAsync_Test()
    {
        // Arrange
        var textToWrite = "Testing WriteAllTextAsync method.";
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Act
            await File.WriteAllTextAsync(tempFilePath, textToWrite);

            // Assert
            var readText = await File.ReadAllTextAsync(tempFilePath);
            readText.Should().Be(textToWrite);
        }
        finally
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // Ignore
            }
        }
    }
}
