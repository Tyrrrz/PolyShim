using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class EnvironmentTests
{
    [Fact]
    public void ProcessPath_Test()
    {
        // Arrange
        using var process = Process.GetCurrentProcess();

        // Act & assert
        Environment.ProcessPath.Should().Be(process.MainModule?.FileName);
    }
}
