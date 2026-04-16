using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace PolyShim.Tests.Net50;

file class MyClass
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public void Foo() { }
}

public class DynamicallyAccessedMembersAttributeTests
{
    [Fact]
    public void Initialization_Test()
    {
        // Arrange
        var instance = new MyClass();

        // Act & assert
        instance.GetType().GetMethod(nameof(MyClass.Foo))!.Invoke(instance, null);
    }
}
