using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PolyShim.Tests.Net90;

public class FileTests
{
    [Fact]
    public async Task AppendAllBytesAsync_Test()
    {
        // Arrange
        var bytesToAppend = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Act
            await File.AppendAllBytesAsync(tempFilePath, bytesToAppend);

            // Assert
            var readBytes = await File.ReadAllBytesAsync(tempFilePath);
            Assert.Equal(bytesToAppend, readBytes);
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
