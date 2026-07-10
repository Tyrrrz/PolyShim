using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class CharTests
{
    [Fact]
    public void IsBetween_Test()
    {
        // Act & assert
        char.IsBetween('c', 'a', 'z').Should().BeTrue();
        char.IsBetween('a', 'a', 'z').Should().BeTrue();
        char.IsBetween('z', 'a', 'z').Should().BeTrue();
        char.IsBetween('A', 'a', 'z').Should().BeFalse();
        char.IsBetween('1', 'a', 'z').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiDigit_Test()
    {
        // Act & assert
        char.IsAsciiDigit('0').Should().BeTrue();
        char.IsAsciiDigit('5').Should().BeTrue();
        char.IsAsciiDigit('9').Should().BeTrue();
        char.IsAsciiDigit('a').Should().BeFalse();
        char.IsAsciiDigit('A').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiHexDigit_Test()
    {
        // Act & assert
        char.IsAsciiHexDigit('0').Should().BeTrue();
        char.IsAsciiHexDigit('9').Should().BeTrue();
        char.IsAsciiHexDigit('a').Should().BeTrue();
        char.IsAsciiHexDigit('f').Should().BeTrue();
        char.IsAsciiHexDigit('A').Should().BeTrue();
        char.IsAsciiHexDigit('F').Should().BeTrue();
        char.IsAsciiHexDigit('g').Should().BeFalse();
        char.IsAsciiHexDigit('G').Should().BeFalse();
        char.IsAsciiHexDigit('z').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiHexDigitLower_Test()
    {
        // Act & assert
        char.IsAsciiHexDigitLower('0').Should().BeTrue();
        char.IsAsciiHexDigitLower('9').Should().BeTrue();
        char.IsAsciiHexDigitLower('a').Should().BeTrue();
        char.IsAsciiHexDigitLower('f').Should().BeTrue();
        char.IsAsciiHexDigitLower('A').Should().BeFalse();
        char.IsAsciiHexDigitLower('F').Should().BeFalse();
        char.IsAsciiHexDigitLower('g').Should().BeFalse();
        char.IsAsciiHexDigitLower('z').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiHexDigitUpper_Test()
    {
        // Act & assert
        char.IsAsciiHexDigitUpper('0').Should().BeTrue();
        char.IsAsciiHexDigitUpper('9').Should().BeTrue();
        char.IsAsciiHexDigitUpper('A').Should().BeTrue();
        char.IsAsciiHexDigitUpper('F').Should().BeTrue();
        char.IsAsciiHexDigitUpper('a').Should().BeFalse();
        char.IsAsciiHexDigitUpper('f').Should().BeFalse();
        char.IsAsciiHexDigitUpper('G').Should().BeFalse();
        char.IsAsciiHexDigitUpper('Z').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiLetter_Test()
    {
        // Act & assert
        char.IsAsciiLetter('a').Should().BeTrue();
        char.IsAsciiLetter('z').Should().BeTrue();
        char.IsAsciiLetter('A').Should().BeTrue();
        char.IsAsciiLetter('Z').Should().BeTrue();
        char.IsAsciiLetter('0').Should().BeFalse();
        char.IsAsciiLetter('!').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiLetterLower_Test()
    {
        // Act & assert
        char.IsAsciiLetterLower('a').Should().BeTrue();
        char.IsAsciiLetterLower('z').Should().BeTrue();
        char.IsAsciiLetterLower('A').Should().BeFalse();
        char.IsAsciiLetterLower('Z').Should().BeFalse();
        char.IsAsciiLetterLower('0').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiLetterUpper_Test()
    {
        // Act & assert
        char.IsAsciiLetterUpper('A').Should().BeTrue();
        char.IsAsciiLetterUpper('Z').Should().BeTrue();
        char.IsAsciiLetterUpper('a').Should().BeFalse();
        char.IsAsciiLetterUpper('z').Should().BeFalse();
        char.IsAsciiLetterUpper('0').Should().BeFalse();
    }

    [Fact]
    public void IsAsciiLetterOrDigit_Test()
    {
        // Act & assert
        char.IsAsciiLetterOrDigit('a').Should().BeTrue();
        char.IsAsciiLetterOrDigit('Z').Should().BeTrue();
        char.IsAsciiLetterOrDigit('5').Should().BeTrue();
        char.IsAsciiLetterOrDigit('!').Should().BeFalse();
        char.IsAsciiLetterOrDigit(' ').Should().BeFalse();
    }
}
