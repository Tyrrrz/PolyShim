using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

file static class Initializer
{
    public static bool IsInitialized { get; private set; }

    [ModuleInitializer]
    public static void Initialize() => IsInitialized = true;
}

public class ModuleInitializerAttributeTests
{
    [Fact]
    public void Initialization_Test()
    {
        // Assert
        Initializer.IsInitialized.Should().BeTrue();
    }
}
