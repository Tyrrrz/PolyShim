using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class OperatingSystemTests
{
    [Fact]
    public void IsLinuxMacOsWindows_Test()
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

    [Fact]
    public void IsWindowsVersionAtLeast_Test()
    {
        // Act & assert
#if PLATFORM_WINDOWS
        // We can assume that these tests will be running on at least Windows 7 (6.1)
        // because Windows 7 is the minimum supported version for the current .NET.
        OperatingSystem.IsWindowsVersionAtLeast(6, 1).Should().BeTrue();

        // We can assume that Windows will never reach version 10000
        OperatingSystem.IsWindowsVersionAtLeast(10000, 0).Should().BeFalse();
#else
        // On non-Windows platforms the method must return false regardless of version
        OperatingSystem.IsWindowsVersionAtLeast(0, 0).Should().BeFalse();
        OperatingSystem.IsWindowsVersionAtLeast(6, 1).Should().BeFalse();
        OperatingSystem.IsWindowsVersionAtLeast(10000, 0).Should().BeFalse();
#endif
    }
}
