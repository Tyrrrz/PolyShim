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
}
