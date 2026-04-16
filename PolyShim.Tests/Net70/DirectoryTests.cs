using System;
using System.IO;
using System.Runtime.Versioning;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class DirectoryTests
{
    [SkippableFact]
    [UnsupportedOSPlatform("windows")]
    public void CreateDirectory_WithUnixCreateMode_SetsDirectoryPermissions_Test()
    {
        Skip.If(OperatingSystem.IsWindows());

        // Arrange
        var tempDirPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            var expectedMode =
                UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute;

            // Act
            var info = Directory.CreateDirectory(tempDirPath, expectedMode);

            // Assert
            info.Should().NotBeNull();
            info.Exists.Should().BeTrue();
            File.GetUnixFileMode(tempDirPath).Should().Be(expectedMode);
        }
        finally
        {
            if (Directory.Exists(tempDirPath))
                Directory.Delete(tempDirPath);
        }
    }
}
