using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class StringSyntaxAttributeTests
{
    [Fact]
    public void Constants_Test()
    {
        // Act & assert
        StringSyntaxAttribute.CSharp.Should().Be("C#");
        StringSyntaxAttribute.FSharp.Should().Be("F#");
        StringSyntaxAttribute.VisualBasic.Should().Be("Visual Basic");
    }

#if !NET7_0_OR_GREATER || NET11_0_OR_GREATER
    private static void CSharpSyntax([StringSyntax(StringSyntaxAttribute.CSharp)] string syntax) { }

    [Fact]
    public void ConstantCanBeUsedInAttribute_Test()
    {
        // Act & assert
        typeof(StringSyntaxAttributeTests)
            .GetMethod(
                nameof(CSharpSyntax),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
            )!
            .GetParameters()[0]
            .GetCustomAttributes(typeof(StringSyntaxAttribute), false)
            .Should()
            .ContainSingle();
    }
#endif
}
