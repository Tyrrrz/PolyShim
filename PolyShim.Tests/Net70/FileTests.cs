using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using FluentAssertions;
using PolyShim.Tests.Utils.Extensions;
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
            File.TryDelete(tempFilePath);
        }
    }

    [SkippableFact]
    [UnsupportedOSPlatform("windows")]
    public void SetUnixFileMode_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        var tempFilePath = Path.GetTempFileName();

        try
        {
            var expectedMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;

            // Act
            File.SetUnixFileMode(tempFilePath, expectedMode);

            // Assert
            var actualMode = File.GetUnixFileMode(tempFilePath);
            actualMode.Should().Be(expectedMode);
        }
        finally
        {
            File.TryDelete(tempFilePath);
        }
    }
}
