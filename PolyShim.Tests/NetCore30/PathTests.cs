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
        Path.TrimEndingDirectorySeparator("C:/Folder/Subfolder/")
            .Should()
            .Be("C:/Folder/Subfolder");
        Path.TrimEndingDirectorySeparator("C:\\Folder\\Subfolder")
            .Should()
            .Be("C:\\Folder\\Subfolder");
        Path.TrimEndingDirectorySeparator("C:/Folder/Subfolder").Should().Be("C:/Folder/Subfolder");
        Path.TrimEndingDirectorySeparator("/a/b/c/").Should().Be("/a/b/c");
        Path.TrimEndingDirectorySeparator("/a/b/c").Should().Be("/a/b/c");
        Path.TrimEndingDirectorySeparator(string.Empty).Should().Be(string.Empty);
    }
}
