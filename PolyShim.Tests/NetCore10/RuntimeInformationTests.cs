using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class RuntimeInformationTests
{
    [Fact]
    public void IsOsPlatform_Test()
    {
        // Act
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        var isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        // Assert
#if PLATFORM_WINDOWS
        isWindows.Should().BeTrue();
        isLinux.Should().BeFalse();
        isOsx.Should().BeFalse();
        #elif PLATFORM_LINUX
        isWindows.Should().BeFalse();
        isLinux.Should().BeTrue();
        isOsx.Should().BeFalse();
#elif PLATFORM_OSX
        isWindows.Should().BeFalse();
        isLinux.Should().BeFalse();
        isOsx.Should().BeTrue();
#else
        isWindows.Should().BeFalse();
        isLinux.Should().BeFalse();
        isOsx.Should().BeFalse();
#endif
    }
}