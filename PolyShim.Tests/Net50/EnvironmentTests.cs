using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class EnvironmentTests
{
    [Fact]
    public void ProcessId_Test()
    {
        // Arrange
        using var process = Process.GetCurrentProcess();

        // Act & assert
        Environment.ProcessId.Should().Be(process.Id);
    }
}
