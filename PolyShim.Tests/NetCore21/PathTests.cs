using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class PathTests
{
    [Fact]
    public void IsPathFullyQualified_Test()
    {
        // Act & assert
        Path.IsPathFullyQualified("C:\\test\\file.txt").Should().BeTrue();
        Path.IsPathFullyQualified("C:/test/file.txt").Should().BeTrue();
        Path.IsPathFullyQualified("\\\\server\\share\\file.txt").Should().BeTrue();
        Path.IsPathFullyQualified("//server/share/file.txt").Should().BeTrue();
        Path.IsPathFullyQualified("C:\\").Should().BeTrue();
        Path.IsPathFullyQualified("C:/").Should().BeTrue();

        Path.IsPathFullyQualified("C:file.txt").Should().BeFalse();
        Path.IsPathFullyQualified("\\test\\file.txt").Should().BeFalse();
        Path.IsPathFullyQualified("/test/file.txt").Should().BeFalse();
        Path.IsPathFullyQualified("test/file.txt").Should().BeFalse();
        Path.IsPathFullyQualified("./test/file.txt").Should().BeFalse();
        Path.IsPathFullyQualified("file.txt").Should().BeFalse();
        Path.IsPathFullyQualified("").Should().BeFalse();
    }
}
