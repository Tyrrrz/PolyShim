using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class ProcessStartInfoTests
{
    [Fact]
    public void ArgumentList_Add_Test()
    {
        // Arrange
        var startInfo = new ProcessStartInfo { UseShellExecute = false, CreateNoWindow = true };

        if (OperatingSystem.IsWindows())
        {
            startInfo.FileName = "cmd";
            startInfo.ArgumentList.Add("/c");
            startInfo.ArgumentList.Add("exit");
            startInfo.ArgumentList.Add("7");
        }
        else
        {
            startInfo.FileName = "sh";
            startInfo.ArgumentList.Add("-c");
            startInfo.ArgumentList.Add("exit 7");
        }

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        // Act
        process.WaitForExit();

        // Assert
        process.ExitCode.Should().Be(7);
    }

    [Fact]
    public void ArgumentList_Add_ArgumentWithSpaces_Test()
    {
        // Arrange
        // "exit 42" is a single argument containing a space.
        // Passing it correctly as one argument makes the shell run `exit 42` (exit code 42).
        // If it were split into two arguments the exit code would differ.
        var startInfo = new ProcessStartInfo { UseShellExecute = false, CreateNoWindow = true };

        if (OperatingSystem.IsWindows())
        {
            startInfo.FileName = "cmd";
            startInfo.ArgumentList.Add("/c");
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
