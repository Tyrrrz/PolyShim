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
                FileName = OperatingSystem.IsWindows() ? "cmd" : "sleep",
                Arguments = OperatingSystem.IsWindows() ? "/c timeout 1" : "1",
                CreateNoWindow = true,
                UseShellExecute = false,
            },
        };

        process.Start();

        // Act
        process.Kill(true);
        var exited = process.WaitForExit(TimeSpan.FromSeconds(5));

        // Assert
        exited.Should().BeTrue();
    }
}
