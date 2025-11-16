using System;
using System.Reflection;
using System.Runtime.Versioning;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class AppContextTests
{
    [Fact]
    public void TargetFrameworkName_Test()
    {
        // Arrange
        var frameworkName = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly())
            .GetCustomAttribute<TargetFrameworkAttribute>()
            ?.FrameworkName;

        // Act & assert
        AppContext.TargetFrameworkName.Should().NotBeNullOrWhiteSpace();
        AppContext.TargetFrameworkName.Should().Be(frameworkName);
    }
}
