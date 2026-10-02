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
    public void CreateDirectory_UnixFileMode_Test()
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

    [Fact]
    public void CreateTempSubdirectory_Test()
    {
        // Act
        var info = Directory.CreateTempSubdirectory();

        try
        {
            // Assert
            info.Should().NotBeNull();
            info.Exists.Should().BeTrue();
            info.FullName.Should()
                .StartWith(Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar));
        }
        finally
        {
            if (Directory.Exists(info.FullName))
                Directory.Delete(info.FullName);
        }
    }

    [Fact]
    public void CreateTempSubdirectory_Prefix_Test()
    {
        // Arrange
        const string prefix = "polyshim-";

        // Act
        var info = Directory.CreateTempSubdirectory(prefix);

        try
        {
            // Assert
            info.Should().NotBeNull();
            info.Exists.Should().BeTrue();
            info.Name.Should().StartWith(prefix);
        }
        finally
        {
            if (Directory.Exists(info.FullName))
                Directory.Delete(info.FullName);
        }
    }

    [Fact]
    public void CreateTempSubdirectory_PrefixWithDirectorySeparator_Test()
    {
        // Act & assert
        Assert
            .Throws<ArgumentException>(() =>
                Directory.CreateTempSubdirectory("foo" + Path.DirectorySeparatorChar + "bar")
            )
            .ParamName.Should()
            .Be("prefix");
    }
}
