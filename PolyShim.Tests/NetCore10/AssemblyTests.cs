using System;
using System.Reflection;
using System.Runtime.Versioning;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class AssemblyTests
{
    [Fact]
    public void GetCustomAttribute_Existing_Test()
    {
        // Arrange
        var assembly = typeof(AssemblyTests).Assembly;

        // Act
        var attribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

        // Assert
        attribute.Should().NotBeNull();
        attribute!.FrameworkName.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetCustomAttribute_Missing_Test()
    {
        // Arrange
        var assembly = typeof(AssemblyTests).Assembly;

        // Act
        var attribute = assembly.GetCustomAttribute<ObsoleteAttribute>();

        // Assert
        attribute.Should().BeNull();
    }
}
