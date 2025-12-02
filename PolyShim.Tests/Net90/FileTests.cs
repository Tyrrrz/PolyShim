using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class FileTests
{
    [Fact]
    public void AppendAllBytes_Test()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();

        try
        {
            File.WriteAllBytes(tempFilePath, [0x00, 0x01, 0x02]);

            // Act
            File.AppendAllBytes(tempFilePath, [0x0A, 0x0B, 0x0C, 0x0D, 0x0E]);

            // Assert
            var readBytes = File.ReadAllBytes(tempFilePath);
            readBytes.Should().Equal(0x00, 0x01, 0x02, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E);
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
    public async Task AppendAllBytesAsync_Test()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();

        try
        {
            await File.WriteAllBytesAsync(tempFilePath, [0x00, 0x01, 0x02]);

            // Act
            await File.AppendAllBytesAsync(tempFilePath, [0x01, 0x02, 0x03, 0x04, 0x05]);

            // Assert
            var readBytes = await File.ReadAllBytesAsync(tempFilePath);
            readBytes.Should().Equal(0x00, 0x01, 0x02, 0x01, 0x02, 0x03, 0x04, 0x05);
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
