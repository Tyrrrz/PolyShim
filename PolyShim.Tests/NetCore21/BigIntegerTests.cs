using System;
using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class BigIntegerTests
{
    [Fact]
    public void ToByteArray_SignedLittleEndian_Test()
    {
        // Arrange
        var value = new BigInteger(33022);

        // Act
        var bytes = value.ToByteArray(isUnsigned: false, isBigEndian: false);

        // Assert
        bytes.Should().Equal(0xFE, 0x80, 0x00);
    }

    [Fact]
    public void ToByteArray_SignedBigEndian_Test()
    {
        // Arrange
        var value = new BigInteger(33022);

        // Act
        var bytes = value.ToByteArray(isUnsigned: false, isBigEndian: true);

        // Assert
        bytes.Should().Equal(0x00, 0x80, 0xFE);
    }

    [Fact]
    public void ToByteArray_UnsignedLittleEndian_Test()
    {
        // Arrange
        var value = new BigInteger(33022);

        // Act
        var bytes = value.ToByteArray(isUnsigned: true, isBigEndian: false);

        // Assert
        bytes.Should().Equal(0xFE, 0x80);
    }

    [Fact]
    public void ToByteArray_UnsignedBigEndian_Test()
    {
        // Arrange
        var value = new BigInteger(33022);

        // Act
        var bytes = value.ToByteArray(isUnsigned: true, isBigEndian: true);

        // Assert
        bytes.Should().Equal(0x80, 0xFE);
    }

    [Fact]
    public void ToByteArray_Zero_Test()
    {
        // Arrange
        var value = BigInteger.Zero;

        // Act
        var bytes = value.ToByteArray(isUnsigned: true, isBigEndian: true);

        // Assert
        bytes.Should().Equal(0x00);
    }

    [Fact]
    public void ToByteArray_NegativeUnsigned_Test()
    {
        // Arrange
        var value = new BigInteger(-1);

        // Act & assert
        var act = () => value.ToByteArray(isUnsigned: true);
        act.Should().Throw<OverflowException>();
    }

    [Fact]
    public void GetByteCount_Test()
    {
        // Arrange
        var value = new BigInteger(33022);

        // Act & assert
        value.GetByteCount().Should().Be(3);
        value.GetByteCount(isUnsigned: true).Should().Be(2);
    }

    [Fact]
    public void TryWriteBytes_Success_Test()
    {
        // Arrange
        var value = new BigInteger(33022);
        var destination = new byte[3];

        // Act
        var result = value.TryWriteBytes(destination, out var bytesWritten);

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(3);
        destination.Should().Equal(0xFE, 0x80, 0x00);
    }

    [Fact]
    public void TryWriteBytes_TooSmallDestination_Test()
    {
        // Arrange
        var value = new BigInteger(33022);
        var destination = new byte[1];

        // Act
        var result = value.TryWriteBytes(destination, out var bytesWritten);

        // Assert
        result.Should().BeFalse();
        bytesWritten.Should().Be(0);
    }

    [Fact]
    public void TryWriteBytes_UnsignedBigEndian_Test()
    {
        // Arrange
        var value = new BigInteger(33022);
        var destination = new byte[2];

        // Act
        var result = value.TryWriteBytes(
            destination,
            out var bytesWritten,
            isUnsigned: true,
            isBigEndian: true
        );

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(2);
        destination.Should().Equal(0x80, 0xFE);
    }
}
