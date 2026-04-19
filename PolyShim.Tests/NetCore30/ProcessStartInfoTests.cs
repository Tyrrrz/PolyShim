using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class ProcessStartInfoTests
{
    [Fact]
    public void ArgumentList_Add_ArgumentWithSpaces_Test()
    {
        // Arrange
        var startInfo = new ProcessStartInfo { UseShellExecute = false, CreateNoWindow = true };

        if (OperatingSystem.IsWindows())
        {
            startInfo.FileName = "powershell";
            startInfo.ArgumentList.Add("-Command");
            startInfo.ArgumentList.Add("exit 42");
        }
        else
        {
            startInfo.FileName = "sh";
            startInfo.ArgumentList.Add("-c");
            startInfo.ArgumentList.Add("exit 42");
        }

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        // Act
        process.WaitForExit();

        // Assert
        process.ExitCode.Should().Be(42);
    }
}
