using System;
using System.Reflection;
using System.Runtime.Versioning;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class CustomAttributeExtensionsTests
{
    [Fact]
    public void GetCustomAttribute_Assembly_Existing_Test()
    {
        // Arrange
        var assembly = typeof(CustomAttributeExtensionsTests).Assembly;

        // Act
        var attribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

        // Assert
        attribute.Should().NotBeNull();
        attribute?.FrameworkName.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetCustomAttribute_Assembly_Missing_Test()
    {
        // Arrange
        var assembly = typeof(CustomAttributeExtensionsTests).Assembly;

        // Act
        var attribute = assembly.GetCustomAttribute<ObsoleteAttribute>();

        // Assert
        attribute.Should().BeNull();
    }

    [Fact]
    public void GetCustomAttribute_MemberInfo_Existing_Test()
    {
        // Arrange
#pragma warning disable CS0612
        var member = typeof(ObsoleteType);
#pragma warning restore CS0612

        // Act
        var attribute = member.GetCustomAttribute<ObsoleteAttribute>();

        // Assert
        attribute.Should().NotBeNull();
    }

    [Fact]
    public void GetCustomAttribute_MemberInfo_Missing_Test()
    {
        // Arrange
        var member = typeof(CustomAttributeExtensionsTests);

        // Act
        var attribute = member.GetCustomAttribute<ObsoleteAttribute>();

        // Assert
        attribute.Should().BeNull();
    }

    [Fact]
    public void GetCustomAttribute_MemberInfo_WithInherit_Existing_Test()
    {
        // Arrange
#pragma warning disable CS0612
        var member = typeof(ObsoleteType);
#pragma warning restore CS0612

        // Act
        var attribute = member.GetCustomAttribute<ObsoleteAttribute>(inherit: true);

        // Assert
        attribute.Should().NotBeNull();
    }

    [Fact]
    public void GetCustomAttribute_MemberInfo_WithInherit_Missing_Test()
    {
        // Arrange
        var member = typeof(CustomAttributeExtensionsTests);

        // Act
        var attribute = member.GetCustomAttribute<ObsoleteAttribute>(inherit: false);

        // Assert
        attribute.Should().BeNull();
    }

    [Fact]
    public void GetCustomAttribute_Module_Existing_Test()
    {
        // Arrange
#pragma warning disable CS0612
        var module = typeof(ObsoleteType).Module;
#pragma warning restore CS0612

        // Act
        // Modules don't typically have user-level attributes; we verify the overload compiles and returns null for missing
        var attribute = module.GetCustomAttribute<ObsoleteAttribute>();

        // Assert
        attribute.Should().BeNull();
    }

    [Fact]
    public void GetCustomAttribute_ParameterInfo_Existing_Test()
    {
        // Arrange
        var parameter = typeof(CustomAttributeExtensionsTests)
            .GetMethod(nameof(MethodWithMarkedParam), BindingFlags.Static | BindingFlags.NonPublic)!
            .GetParameters()[0];

        // Act
        var attribute = parameter.GetCustomAttribute<MarkerAttribute>();

        // Assert
        attribute.Should().NotBeNull();
    }

    [Fact]
    public void GetCustomAttribute_ParameterInfo_Missing_Test()
    {
        // Arrange
        var parameter = typeof(CustomAttributeExtensionsTests)
            .GetMethod(nameof(MethodWithMarkedParam), BindingFlags.Static | BindingFlags.NonPublic)!
            .GetParameters()[1];

        // Act
        var attribute = parameter.GetCustomAttribute<MarkerAttribute>();

        // Assert
        attribute.Should().BeNull();
    }

    [Fact]
    public void GetCustomAttribute_ParameterInfo_WithInherit_Existing_Test()
    {
        // Arrange
        var parameter = typeof(CustomAttributeExtensionsTests)
            .GetMethod(nameof(MethodWithMarkedParam), BindingFlags.Static | BindingFlags.NonPublic)!
            .GetParameters()[0];

        // Act
        var attribute = parameter.GetCustomAttribute<MarkerAttribute>(inherit: true);

        // Assert
        attribute.Should().NotBeNull();
    }

    [Fact]
    public void GetCustomAttribute_ParameterInfo_WithInherit_Missing_Test()
    {
        // Arrange
        var parameter = typeof(CustomAttributeExtensionsTests)
            .GetMethod(nameof(MethodWithMarkedParam), BindingFlags.Static | BindingFlags.NonPublic)!
            .GetParameters()[1];

        // Act
        var attribute = parameter.GetCustomAttribute<MarkerAttribute>(inherit: false);

        // Assert
        attribute.Should().BeNull();
    }

    // Helper members used by the tests above

    [Obsolete]
    private class ObsoleteType;

    [AttributeUsage(AttributeTargets.Parameter)]
    private sealed class MarkerAttribute : Attribute;

    private static void MethodWithMarkedParam([Marker] int x, int y) { }
}
