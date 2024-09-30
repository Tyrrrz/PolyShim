using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class TypeTests
{
    private interface IBaseInterface;

    private class BaseClass : IBaseInterface;

    private class DerivedClass : BaseClass;

    [Fact]
    public void IsAssignableFrom_Test()
    {
        typeof(DerivedClass).IsAssignableFrom(typeof(DerivedClass)).Should().BeTrue();
        typeof(DerivedClass).IsAssignableFrom(typeof(BaseClass)).Should().BeFalse();
        typeof(DerivedClass).IsAssignableFrom(typeof(IBaseInterface)).Should().BeFalse();
        typeof(BaseClass).IsAssignableFrom(typeof(BaseClass)).Should().BeTrue();
        typeof(BaseClass).IsAssignableFrom(typeof(DerivedClass)).Should().BeTrue();
        typeof(BaseClass).IsAssignableFrom(typeof(IBaseInterface)).Should().BeFalse();
        typeof(IBaseInterface).IsAssignableFrom(typeof(IBaseInterface)).Should().BeTrue();
        typeof(IBaseInterface).IsAssignableFrom(typeof(DerivedClass)).Should().BeTrue();
        typeof(IBaseInterface).IsAssignableFrom(typeof(BaseClass)).Should().BeTrue();
    }

    [Fact]
    public void IsSubclassOf_Test()
    {
        typeof(DerivedClass).IsSubclassOf(typeof(DerivedClass)).Should().BeFalse();
        typeof(DerivedClass).IsSubclassOf(typeof(BaseClass)).Should().BeTrue();
        typeof(DerivedClass).IsSubclassOf(typeof(IBaseInterface)).Should().BeFalse();
        typeof(BaseClass).IsSubclassOf(typeof(BaseClass)).Should().BeFalse();
        typeof(BaseClass).IsSubclassOf(typeof(DerivedClass)).Should().BeFalse();
        typeof(BaseClass).IsSubclassOf(typeof(IBaseInterface)).Should().BeFalse();
        typeof(IBaseInterface).IsSubclassOf(typeof(IBaseInterface)).Should().BeFalse();
        typeof(IBaseInterface).IsSubclassOf(typeof(DerivedClass)).Should().BeFalse();
        typeof(IBaseInterface).IsSubclassOf(typeof(BaseClass)).Should().BeFalse();
    }
}
