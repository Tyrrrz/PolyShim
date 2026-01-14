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
#if PLATFORM_WINDOWS
        Path.IsPathFullyQualified(@"C:\Folder\File.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"C:/Folder/File.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"\Folder\File.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"/Folder/File.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"\\Server\Share\File.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"//Server/Share/File.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"C:Folder\File.txt").Should().BeFalse();
        Path.IsPathFullyQualified(@"C:Folder/File.txt").Should().BeFalse();
        Path.IsPathFullyQualified(@"Folder\File.txt").Should().BeFalse();
        Path.IsPathFullyQualified(@"Folder/File.txt").Should().BeFalse();
        Path.IsPathFullyQualified(@"./Folder/File.txt").Should().BeFalse();
#else
        Path.IsPathFullyQualified(@"/home/user/directory/file.txt").Should().BeTrue();
        Path.IsPathFullyQualified(@"home/user/directory/file.txt").Should().BeFalse();
        Path.IsPathFullyQualified(@"./directory/file.txt").Should().BeFalse();
#endif
    }
}
