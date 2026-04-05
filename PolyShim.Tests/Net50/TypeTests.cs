using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

file interface IBaseInterface;

file class BaseClass : IBaseInterface;

file class DerivedClass : BaseClass;

public class TypeTests
{
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
