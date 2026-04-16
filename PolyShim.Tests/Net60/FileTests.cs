using System.IO;
using System.Runtime.Versioning;
using FluentAssertions;
using PolyShim.Tests.Utils.Extensions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class FileTests
{
    [Fact]
    public void Open_WithDefaultOptions_ReturnsReadableStream_Test()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllBytes(tempFilePath, [0x00, 0x01, 0x02]);

        try
        {
            var options = new FileStreamOptions();

            // Act
            using var stream = File.Open(tempFilePath, options);

            // Assert
            stream.CanRead.Should().BeTrue();
            stream.Length.Should().Be(3);
        }
        finally
        {
            File.TryDelete(tempFilePath);
        }
    }

    [Fact]
    public void Open_WithWriteOptions_ReturnsWritableStream_Test()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();

        try
        {
            var options = new FileStreamOptions
            {
                Mode = FileMode.Create,
                Access = FileAccess.Write,
                Share = FileShare.None,
            };

            // Act
            using var stream = File.Open(tempFilePath, options);
            stream.Write([0x0A, 0x0B, 0x0C], 0, 3);

            // Assert
            stream.CanWrite.Should().BeTrue();
            stream.Position.Should().Be(3);
        }
        finally
        {
            File.TryDelete(tempFilePath);
        }
    }

    [Fact]
    public void Open_WithReadWriteOptions_ReturnsReadWriteStream_Test()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllBytes(tempFilePath, [0x00, 0x01, 0x02]);

        try
        {
            var options = new FileStreamOptions
            {
                Mode = FileMode.Open,
                Access = FileAccess.ReadWrite,
                Share = FileShare.None,
            };

            // Act
            using var stream = File.Open(tempFilePath, options);

            // Assert
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeTrue();
        }
        finally
        {
            File.TryDelete(tempFilePath);
        }
    }

#if !PLATFORM_WINDOWS
    [Fact]
    [UnsupportedOSPlatform("windows")]
    public void Open_WithUnixCreateMode_SetsFilePermissions_Test()
    {
        // Arrange
        var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            var expectedMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;

            var options = new FileStreamOptions
            {
                Mode = FileMode.Create,
                Access = FileAccess.Write,
                Share = FileShare.None,
                UnixCreateMode = expectedMode,
            };

            // Act
            using (var stream = File.Open(tempFilePath, options))
                stream.Write([0x0A, 0x0B], 0, 2);

            // Assert
            var actualMode = File.GetUnixFileMode(tempFilePath);
            actualMode.Should().Be(expectedMode);
        }
        finally
        {
            File.TryDelete(tempFilePath);
        }
    }
#endif
}
