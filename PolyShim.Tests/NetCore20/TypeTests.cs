using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class TypeTests
{
    private class BaseClass
    {
    }

    private class DerivedClass : BaseClass
    {
    }

    [Fact]
    public void IsSubclassOf_Test()
    {
        typeof(DerivedClass).IsSubclassOf(typeof(BaseClass)).Should().BeTrue();
        typeof(DerivedClass).IsSubclassOf(typeof(DerivedClass)).Should().BeFalse();
        typeof(BaseClass).IsSubclassOf(typeof(DerivedClass)).Should().BeFalse();
    }
}