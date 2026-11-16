using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class TypeTests
{
    [Fact]
    public void GetNullableUnderlyingType_Test()
    {
        // Act & assert
        typeof(int?).GetNullableUnderlyingType().Should().Be(typeof(int));
        typeof(int).GetNullableUnderlyingType().Should().BeNull();
    }
}
