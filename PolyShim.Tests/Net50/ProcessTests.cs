﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class ProcessTests
{
    [Fact]
    public async Task WaitForExitAsync_Test()
    {
        // Arrange
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd" : "sh",
                Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "/c timeout 1"
                    : "-c 'sleep 1'",
                CreateNoWindow = true,
                UseShellExecute = false,
            },
        };

        process.Start();

        // Act
        await process.WaitForExitAsync();

        // Assert
        process.HasExited.Should().BeTrue();
    }
}
