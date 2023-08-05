using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class TypeTests
{
    private class BaseClass
    {
    }

    private class DerivedClass : BaseClass
    {
    }

    [Fact]
    public void IsAssignableTo_Test()
    {
        typeof(DerivedClass).IsAssignableTo(typeof(BaseClass)).Should().BeTrue();
        typeof(DerivedClass).IsAssignableTo(typeof(DerivedClass)).Should().BeTrue();
        typeof(BaseClass).IsAssignableTo(typeof(DerivedClass)).Should().BeFalse();
    }
}