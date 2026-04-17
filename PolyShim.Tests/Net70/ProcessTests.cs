using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class ProcessTests
{
    [Fact]
    public void WaitForExit_TimeSpan_Test()
    {
        // Arrange
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = OperatingSystem.IsWindows() ? "cmd" : "sh",
                Arguments = OperatingSystem.IsWindows() ? "/c timeout 1" : "-c 'sleep 1'",
                CreateNoWindow = true,
                UseShellExecute = false,
            },
        };

        process.Start();

        // Act
        var exited = process.WaitForExit(TimeSpan.FromSeconds(10));

        // Assert
        exited.Should().BeTrue();
        process.HasExited.Should().BeTrue();
    }

    [Fact]
    public void WaitForExit_TimeSpan_Timeout_Test()
    {
        // Arrange
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = OperatingSystem.IsWindows() ? "cmd" : "sleep",
                Arguments = OperatingSystem.IsWindows() ? "/c timeout 30 /nobreak" : "30",
                CreateNoWindow = true,
                UseShellExecute = false,
            },
        };
        process.Start();

        try
        {
            // Act
            var exited = process.WaitForExit(TimeSpan.FromMilliseconds(100));

            // Assert
            exited.Should().BeFalse();
        }
        finally
        {
            process.Kill();
        }
    }
}
