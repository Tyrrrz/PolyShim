using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class PathTests
{
    [Fact]
    public void Exists_File_Test()
    {
        // Arrange
        var path = Path.GetTempFileName();
        try
        {
            // Act & assert
            Path.Exists(path).Should().BeTrue();
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Exists_Directory_Test()
    {
        // Arrange
        var path = Path.GetTempPath();

        // Act & assert
        Path.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void Exists_NotFound_Test()
    {
        // Arrange
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        // Act & assert
        Path.Exists(path).Should().BeFalse();
    }

    [Fact]
    public void Exists_Null_Test()
    {
        // Act & assert
        Path.Exists(null).Should().BeFalse();
    }

    [Fact]
    public void Exists_Empty_Test()
    {
        // Act & assert
        Path.Exists("").Should().BeFalse();
    }
}
