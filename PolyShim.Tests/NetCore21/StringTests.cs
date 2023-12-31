﻿using System;
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
}
