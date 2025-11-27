using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class RegexTests
{
    [Fact]
    public void Count_Test()
    {
        // Arrange
        const string input = "There are 3 numbers: 42 and 100";

        // Act & assert
        new Regex(@"\d+")
            .Count(input)
            .Should()
            .Be(3);
        Regex.Count(input, @"\d+").Should().Be(3);
    }

    [Fact]
    public void Count_Options_Test()
    {
        // Arrange
        const string input = "There are 3 numbers: 42 and 100";

        // Act & assert
        Regex.Count(input, @"\d+", RegexOptions.None).Should().Be(3);
    }

    [Fact]
    public void Count_OptionsAndTimeout_Test()
    {
        // Arrange
        const string input = "There are 3 numbers: 42 and 100";

        // Act & assert
        Regex.Count(input, @"\d+", RegexOptions.None, TimeSpan.FromSeconds(1)).Should().Be(3);
    }
}
