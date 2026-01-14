using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class ProcessTests
{
    [Fact]
    public void Kill_Test()
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
        process.Kill(true);

        // Assert
        process.HasExited.Should().BeTrue();
    }
}
