using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class StringTests
{
    [Fact]
    public void ReplaceLineEndings_Test()
    {
        // Arrange
        const string str = "abc\r\ndef\rghi\njkl\r\nmno\rpqr\nstu";

        // Act & assert
        str.ReplaceLineEndings()
            .Should()
            .Be(
                "abc"
                    + Environment.NewLine
                    + "def"
                    + Environment.NewLine
                    + "ghi"
                    + Environment.NewLine
                    + "jkl"
                    + Environment.NewLine
                    + "mno"
                    + Environment.NewLine
                    + "pqr"
                    + Environment.NewLine
                    + "stu"
            );
    }

    [Fact]
    public void ReplaceLineEndings_ReplacementText_Test()
    {
        // Arrange
        const string str = "abc\r\ndef\rghi\njkl\r\nmno\rpqr\nstu";

        // Act & assert
        str.ReplaceLineEndings(" ").Should().Be("abc def ghi jkl mno pqr stu");
    }
}
