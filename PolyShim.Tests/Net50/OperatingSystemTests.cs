using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class OperatingSystemTests
{
    [Fact]
    public void IsXyz_Test()
    {
        // Act
        var isLinux = OperatingSystem.IsLinux();
        var isMacOs = OperatingSystem.IsMacOS();
        var isWindows = OperatingSystem.IsWindows();

        // Assert
#if PLATFORM_WINDOWS
        isLinux.Should().BeFalse();
        isMacOs.Should().BeFalse();
        isWindows.Should().BeTrue();
#elif PLATFORM_LINUX
        isLinux.Should().BeTrue();
        isMacOs.Should().BeFalse();
        isWindows.Should().BeFalse();
#elif PLATFORM_OSX
        isLinux.Should().BeFalse();
        isMacOs.Should().BeTrue();
        isWindows.Should().BeFalse();
#else
        isLinux.Should().BeFalse();
        isMacOs.Should().BeFalse();
        isWindows.Should().BeFalse();
#endif
    }
}
