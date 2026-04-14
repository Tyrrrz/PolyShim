using System;
using System.Buffers;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class StringTests
{
    [Fact]
    public void Contains_Char_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.Contains('B', StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        str.Contains('B', StringComparison.Ordinal).Should().BeFalse();
    }

    [Fact]
    public void Contains_String_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.Contains("B", StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        str.Contains("B", StringComparison.Ordinal).Should().BeFalse();
    }

    [Fact]
    public void Create_Test()
    {
        // Act & assert
        string.Create(
                5,
                'x',
                (SpanAction<char, char>)(
                    (span, c) =>
                    {
                        for (var i = 0; i < span.Length; i++)
                            span[i] = c;
                    }
                )
            )
            .Should()
            .Be("xxxxx");
    }

    [Fact]
    public void Create_Empty_Test()
    {
        // Act & assert
        string.Create(0, 0, (SpanAction<char, int>)((_, _) => { })).Should().Be(string.Empty);
    }

    [Fact]
    public void Create_NegativeLength_Test()
    {
        // Act & assert
        var act = () => string.Create(-1, 0, (SpanAction<char, int>)((_, _) => { }));
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_NullAction_Test()
    {
        // Act & assert
        var act = () => string.Create(5, 0, (SpanAction<char, int>)null!);
        act.Should().Throw<ArgumentNullException>();
    }
}
