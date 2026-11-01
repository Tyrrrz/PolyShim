using System;
using System.Text.Json;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class JsonElementTests
{
    [Fact]
    public void Parse_String_Test()
    {
        // Act
        var element = JsonElement.Parse("{\"a\":1}");

        // Assert
        element.GetProperty("a").GetInt32().Should().Be(1);
    }

    [Fact]
    public void Parse_CharSpan_Test()
    {
        // Act
        var element = JsonElement.Parse("[1,2,3]".AsSpan());

        // Assert
        element.GetArrayLength().Should().Be(3);
    }

    [Fact]
    public void Parse_Utf8Span_Test()
    {
        // Act
        var element = JsonElement.Parse("\"hello\""u8);

        // Assert
        element.GetString().Should().Be("hello");
    }
}
