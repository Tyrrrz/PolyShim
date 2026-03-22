using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class NumericTryParseSpanTests
{
    [Fact]
    public void Byte_TryParse_Span_Test()
    {
        // Act & assert
        byte.TryParse("42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(42);

        byte.TryParse("999".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void Short_TryParse_Span_Test()
    {
        // Act & assert
        short.TryParse("1000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(1000);

        short.TryParse("99999".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void Int_TryParse_Span_Test()
    {
        // Act & assert
        int.TryParse("12345".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(12345);

        int.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void Long_TryParse_Span_Test()
    {
        // Act & assert
        long.TryParse("9876543210".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(9876543210L);

        long.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void SByte_TryParse_Span_Test()
    {
        // Act & assert
        sbyte.TryParse("-42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(-42);
    }

    [Fact]
    public void UShort_TryParse_Span_Test()
    {
        // Act & assert
        ushort.TryParse("1000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(1000);
    }

    [Fact]
    public void UInt_TryParse_Span_Test()
    {
        // Act & assert
        uint.TryParse("4000000000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(4000000000U);
    }

    [Fact]
    public void ULong_TryParse_Span_Test()
    {
        // Act & assert
        ulong.TryParse("18000000000000000000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(18000000000000000000UL);
    }

    [Fact]
    public void Float_TryParse_Span_Test()
    {
        // Act & assert
        float.TryParse("3.14".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Should().BeApproximately(3.14f, 0.001f);

        float.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void Double_TryParse_Span_Test()
    {
        // Act & assert
        double.TryParse("3.14159".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Should().BeApproximately(3.14159, 0.00001);

        double.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void Decimal_TryParse_Span_Test()
    {
        // Act & assert
        decimal.TryParse("123.45".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Should().Be(123.45m);

        decimal.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void DateTime_TryParse_Span_Test()
    {
        // Act & assert
        DateTime
            .TryParse("2024-01-15".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Year.Should().Be(2024);
        result.Month.Should().Be(1);
        result.Day.Should().Be(15);

        DateTime.TryParse("not a date".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void DateTimeOffset_TryParse_Span_Test()
    {
        // Act & assert
        DateTimeOffset
            .TryParse(
                "2024-01-15T12:00:00+00:00".AsSpan(),
                CultureInfo.InvariantCulture,
                out var result
            )
            .Should()
            .BeTrue();
        result.Year.Should().Be(2024);

        DateTimeOffset.TryParse("not a date".AsSpan(), null, out _).Should().BeFalse();
    }

    [Fact]
    public void IntPtr_TryParse_Span_Test()
    {
        // Act & assert
        IntPtr.TryParse("42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(new IntPtr(42));
    }

    [Fact]
    public void UIntPtr_TryParse_Span_Test()
    {
        // Act & assert
        UIntPtr.TryParse("42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(new UIntPtr(42));
    }
}
