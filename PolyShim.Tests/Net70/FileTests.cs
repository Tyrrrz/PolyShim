using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class FileTests
{
    [Fact]
    public async Task ReadLinesAsync_Test()
    {
        // Arrange
        var linesToWrite = new[] { "Line 1", "Line 2", "Line 3" };
        var tempFilePath = Path.GetTempFileName();
        await File.WriteAllLinesAsync(tempFilePath, linesToWrite);

        try
        {
            // Act
            var readLines = new List<string>();
            await foreach (var line in File.ReadLinesAsync(tempFilePath))
                readLines.Add(line);

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
}
