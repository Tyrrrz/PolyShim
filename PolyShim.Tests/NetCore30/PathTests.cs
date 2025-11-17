using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class PathTests
{
    [Fact]
    public void EndsInDirectorySeparator_Test()
    {
        // Act & assert
        Path.EndsInDirectorySeparator("C:\\Folder\\Subfolder\\").Should().BeTrue();
        Path.EndsInDirectorySeparator("C:/Folder/Subfolder/").Should().BeTrue();
        Path.EndsInDirectorySeparator("C:\\Folder\\Subfolder").Should().BeFalse();
        Path.EndsInDirectorySeparator("C:/Folder/Subfolder").Should().BeFalse();
        Path.EndsInDirectorySeparator("/a/b/c/").Should().BeTrue();
        Path.EndsInDirectorySeparator("/a/b/c").Should().BeFalse();
        Path.EndsInDirectorySeparator(string.Empty).Should().BeFalse();
    }

    [Fact]
    public void TrimEndingDirectorySeparator_Test()
    {
        // Act & assert
        Path.TrimEndingDirectorySeparator("C:\\Folder\\Subfolder\\")
            .Should()
            .Be("C:\\Folder\\Subfolder");
        Path.TrimEndingDirectorySeparator("C:\\Folder\\Subfolder\\\\")
            .Should()
            .Be("C:\\Folder\\Subfolder\\");
        Path.TrimEndingDirectorySeparator("C:/Folder/Subfolder/")
            .Should()
            .Be("C:/Folder/Subfolder");
        Path.TrimEndingDirectorySeparator("C:\\Folder\\Subfolder")
            .Should()
            .Be("C:\\Folder\\Subfolder");
        Path.TrimEndingDirectorySeparator("C:/Folder/Subfolder").Should().Be("C:/Folder/Subfolder");
        Path.TrimEndingDirectorySeparator("/a/b/c/").Should().Be("/a/b/c");
        Path.TrimEndingDirectorySeparator("/a/b/c//").Should().Be("/a/b/c/");
        Path.TrimEndingDirectorySeparator("/a/b/c").Should().Be("/a/b/c");
        Path.TrimEndingDirectorySeparator(string.Empty).Should().Be(string.Empty);
    }

    [Fact]
    public void Join_Test()
    {
        // Act & assert

        // For some reason, dropping the array literals leads to nullability errors on .NET Framework
        // ReSharper disable RedundantExplicitParamsArrayCreation
        Path.Join(["C:/Program Files/", "Utilities/SystemUtilities"])
            .Should()
            .Be("C:/Program Files/Utilities/SystemUtilities");
        Path.Join(["C:/", "/Program Files"]).Should().Be("C://Program Files");
        Path.Join(["C:/Users/Public/Documents/", "C:/Users/User1/Documents/Financial/"])
            .Should()
            .Be("C:/Users/Public/Documents/C:/Users/User1/Documents/Financial/");
        // ReSharper restore RedundantExplicitParamsArrayCreation
    }
}
