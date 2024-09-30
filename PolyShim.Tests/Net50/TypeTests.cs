using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class TypeTests
{
    private interface IBaseInterface;

    private class BaseClass : IBaseInterface;

    private class DerivedClass : BaseClass;

    [Fact]
    public void IsAssignableTo_Test()
    {
        typeof(DerivedClass).IsAssignableTo(typeof(DerivedClass)).Should().BeTrue();
        typeof(DerivedClass).IsAssignableTo(typeof(BaseClass)).Should().BeTrue();
        typeof(DerivedClass).IsAssignableTo(typeof(IBaseInterface)).Should().BeTrue();
        typeof(BaseClass).IsAssignableTo(typeof(BaseClass)).Should().BeTrue();
        typeof(BaseClass).IsAssignableTo(typeof(DerivedClass)).Should().BeFalse();
        typeof(BaseClass).IsAssignableTo(typeof(IBaseInterface)).Should().BeTrue();
        typeof(IBaseInterface).IsAssignableTo(typeof(IBaseInterface)).Should().BeTrue();
        typeof(IBaseInterface).IsAssignableTo(typeof(DerivedClass)).Should().BeFalse();
        typeof(IBaseInterface).IsAssignableTo(typeof(BaseClass)).Should().BeFalse();
    }
}
