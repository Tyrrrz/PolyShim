using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class PathTests
{
    [Fact]
    public void GetRelativePath_Test()
    {
        // Act & assert
        Path.GetRelativePath("/a/b/c", "/a/b/c").Should().Be(".");

        Path.GetRelativePath("/a/b/c", "/a/b/c/d/e.txt").Should().Be(Path.Combine("d", "e.txt"));

        Path.GetRelativePath("/a/b/c", "/a/b/x/y/z.txt")
            .Should()
            .Be(Path.Combine("..", "x", "y", "z.txt"));

        Path.GetRelativePath("/a/b/c/", "/d/e/f.txt")
            .Should()
            .Be(Path.Combine("..", "..", "..", "d", "e", "f.txt"));

        Path.GetRelativePath("C:\\a\\b\\c", "C:\\a\\b\\c").Should().Be(".");

        Path.GetRelativePath("C:\\a\\b\\c", "C:\\a\\b\\c\\d\\e.txt")
            .Should()
            .Be(Path.Combine("d", "e.txt"));

        Path.GetRelativePath("C:\\a\\b\\c", "C:\\a\\b\\x\\y\\z.txt")
            .Should()
            .Be(Path.Combine("..", "x", "y", "z.txt"));

        Path.GetRelativePath("C:\\a\\b\\c\\", "D:\\d\\e\\f.txt")
            .Should()
            .Be(Path.Combine("D:\\", "d", "e", "f.txt"));
    }
}
