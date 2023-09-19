using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace PolyShim.Tests.Net50;

public class DynamicallyAccessedMembersAttributeTests
{
    private class MyClass
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
        public void Foo() { }
    }

    [Fact]
    public void Initialization_Test()
    {
        // Arrange
        var instance = new MyClass();

        // Act & assert
        instance.GetType().GetMethod(nameof(MyClass.Foo))!.Invoke(instance, null);
    }
}
