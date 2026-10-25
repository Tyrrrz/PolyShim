using System;
using System.Buffers.Text;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class Base64UrlTests
{
    [Fact]
    public void IsValid_Test()
    {
        // Act & assert
        Base64Url.IsValid("".AsSpan()).Should().BeTrue();
        Base64Url.IsValid("SGVsbG8gd29ybGQh".AsSpan()).Should().BeTrue();
        Base64Url.IsValid("SGVsbG8gd29ybGQh".AsSpan()).Should().BeTrue();
        Base64Url.IsValid("SGVsbG8gd29ybGQh!".AsSpan()).Should().BeFalse();
        Base64Url.IsValid(""u8).Should().BeTrue();
        Base64Url.IsValid("SGVsbG8gd29ybGQh"u8).Should().BeTrue();
        Base64Url.IsValid("SGVsbG8gd29ybGQh"u8).Should().BeTrue();
        Base64Url.IsValid("SGVsbG8gd29ybGQh!"u8).Should().BeFalse();
    }

    [Fact]
    public void GetEncodedLength_Test()
    {
        // Act & assert
        Base64Url.GetEncodedLength(0).Should().Be(0);
        Base64Url.GetEncodedLength(1).Should().Be(2);
        Base64Url.GetEncodedLength(2).Should().Be(3);
        Base64Url.GetEncodedLength(12).Should().Be(16);
        Base64Url.GetEncodedLength(32).Should().Be(43);
        Base64Url.GetEncodedLength(41).Should().Be(55);
        Base64Url.GetEncodedLength(77).Should().Be(103);
    }

    [Fact]
    public void GetMaxDecodedLength_Test()
    {
        // Act & assert
        Base64Url.GetMaxDecodedLength(0).Should().Be(0);
        Base64Url.GetMaxDecodedLength(2).Should().Be(1);
        Base64Url.GetMaxDecodedLength(3).Should().Be(2);
        Base64Url.GetMaxDecodedLength(4).Should().Be(3);
        Base64Url.GetMaxDecodedLength(16).Should().Be(12);
        Base64Url.GetMaxDecodedLength(43).Should().Be(32);
        Base64Url.GetMaxDecodedLength(55).Should().Be(41);
        Base64Url.GetMaxDecodedLength(103).Should().Be(77);
    }

    [Fact]
    public void DecodeFromChars_Test()
    {
        // Act & assert
        Base64Url.DecodeFromChars("").Should().BeEmpty();
        Base64Url
            .DecodeFromChars("SGVsbG8gd29ybGQh".AsSpan())
            .Should()
            .Equal([
                (byte)'H',
                (byte)'e',
                (byte)'l',
                (byte)'l',
                (byte)'o',
                (byte)' ',
                (byte)'w',
                (byte)'o',
                (byte)'r',
                (byte)'l',
                (byte)'d',
                (byte)'!',
            ]);
    }

    [Fact]
    public void DecodeFromChars_WithDestination_Test()
    {
        // Arrange
        var destination = new byte[256];

        // Act
        var bytesWritten = Base64Url.DecodeFromChars("SGVsbG8gd29ybGQh".AsSpan(), destination);

        // Assert
        bytesWritten.Should().Be(12);
        destination
            .AsSpan(0, bytesWritten)
            .Should()
            .Equal([
                (byte)'H',
                (byte)'e',
                (byte)'l',
                (byte)'l',
                (byte)'o',
                (byte)' ',
                (byte)'w',
                (byte)'o',
                (byte)'r',
                (byte)'l',
                (byte)'d',
                (byte)'!',
            ]);
    }

    [Fact]
    public void DecodeFromChars_WithDestination_Overflow_Test()
    {
        // Arrange
        var destination = new byte[1];

        // Act
        var act = () => Base64Url.DecodeFromChars("SGVsbG8gd29ybGQh".AsSpan(), destination);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DecodeFromUtf8_Test()
    {
        // Act & assert
        Base64Url.DecodeFromUtf8(""u8).Should().BeEmpty();
        Base64Url
            .DecodeFromUtf8("SGVsbG8gd29ybGQh"u8)
            .Should()
            .Equal([
                (byte)'H',
                (byte)'e',
                (byte)'l',
                (byte)'l',
                (byte)'o',
                (byte)' ',
                (byte)'w',
                (byte)'o',
                (byte)'r',
                (byte)'l',
                (byte)'d',
                (byte)'!',
            ]);
    }

    [Fact]
    public void DecodeFromUtf8_WithDestination_Test()
    {
        // Arrange
        var destination = new byte[256];

        // Act
        var bytesWritten = Base64Url.DecodeFromUtf8("SGVsbG8gd29ybGQh"u8, destination);

        // Assert
        bytesWritten.Should().Be(12);
        destination
            .AsSpan(0, bytesWritten)
            .Should()
            .Equal([
                (byte)'H',
                (byte)'e',
                (byte)'l',
                (byte)'l',
                (byte)'o',
                (byte)' ',
                (byte)'w',
                (byte)'o',
                (byte)'r',
                (byte)'l',
                (byte)'d',
                (byte)'!',
            ]);
    }

    [Fact]
    public void DecodeFromUtf8_WithDestination_Overflow_Test()
    {
        // Arrange
        var destination = new byte[1];

        // Act
        var act = () => Base64Url.DecodeFromUtf8("SGVsbG8gd29ybGQh"u8, destination);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DecodeFromUtf8InPlace_Test()
    {
        // Arrange
        var buffer = "SGVsbG8gd29ybGQh"u8.ToArray();

        // Act
        var decodedLength = Base64Url.DecodeFromUtf8InPlace(buffer);

        // Assert
        decodedLength.Should().Be(12);
        buffer
            .AsSpan(0, decodedLength)
            .Should()
            .Equal([
                (byte)'H',
                (byte)'e',
                (byte)'l',
                (byte)'l',
                (byte)'o',
                (byte)' ',
                (byte)'w',
                (byte)'o',
                (byte)'r',
                (byte)'l',
                (byte)'d',
                (byte)'!',
            ]);
    }

    [Fact]
    public void EncodeToChars_Test()
    {
        // Act & assert
        Base64Url.EncodeToChars([]).Should().Equal([]);
        Base64Url
            .EncodeToChars([1, 2, 3, 4, 5, 6, 7, 8, 9])
            .Should()
            .Equal("AQIDBAUGBwgJ".ToCharArray());
        Base64Url
            .EncodeToChars([72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33])
            .Should()
            .Equal("SGVsbG8gd29ybGQh".ToCharArray());
    }

    [Fact]
    public void EncodeToChars_WithDestination_Test()
    {
        // Arrange
        var destination = new char[256];

        // Act
        var encodedLength = Base64Url.EncodeToChars(
            [72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33],
            destination
        );

        // Assert
        encodedLength.Should().Be(16);
        destination.AsSpan(0, encodedLength).Should().Equal("SGVsbG8gd29ybGQh".ToCharArray());
    }

    [Fact]
    public void EncodeToChars_WithDestination_Overflow_Test()
    {
        // Arrange
        var destination = new char[1];

        // Act
        var act = () =>
            Base64Url.EncodeToChars(
                [72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33],
                destination
            );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void EncodeToUtf8_Test()
    {
        // Act & assert
        Base64Url.EncodeToUtf8([]).Should().BeEmpty();
        Base64Url
            .EncodeToUtf8([1, 2, 3, 4, 5, 6, 7, 8, 9])
            .Should()
            .Equal("AQIDBAUGBwgJ"u8.ToArray());
        Base64Url
            .EncodeToUtf8([72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33])
            .Should()
            .Equal("SGVsbG8gd29ybGQh"u8.ToArray());
    }

    [Fact]
    public void EncodeToUtf8_WithDestination_Test()
    {
        // Arrange
        var destination = new byte[256];

        // Act
        var encodedLength = Base64Url.EncodeToUtf8(
            [72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33],
            destination
        );

        // Assert
        encodedLength.Should().Be(16);
        destination.AsSpan(0, encodedLength).Should().Equal("SGVsbG8gd29ybGQh"u8.ToArray());
    }

    [Fact]
    public void EncodeToUtf8_WithDestination_Overflow_Test()
    {
        // Arrange
        var destination = new byte[1];

        // Act
        var act = () =>
            Base64Url.EncodeToUtf8(
                [72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33],
                destination
            );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void EncodeToString_Test()
    {
        // Act & assert
        Base64Url.EncodeToString([]).Should().Be("");
        Base64Url.EncodeToString([1, 2, 3, 4, 5, 6, 7, 8, 9]).Should().Be("AQIDBAUGBwgJ");
        Base64Url
            .EncodeToString([72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33])
            .Should()
            .Be("SGVsbG8gd29ybGQh");
    }
}
