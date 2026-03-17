using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class StringTests
{
    [Fact]
    public void Join_StringSeparator_ObjectArray_Test()
    {
        // Act & assert
        string.Join(", ", new object?[] { 1, "two", 3.0 }).Should().Be("1, two, 3");
        string.Join(", ", new object?[] { }).Should().Be("");
        string.Join(", ", new object?[] { null, "a", null }).Should().Be(", a, ");
    }

    [Fact]
    public void Join_StringSeparator_EnumerableT_Test()
    {
        // Act & assert
        string.Join(", ", new List<int> { 1, 2, 3 }).Should().Be("1, 2, 3");
        string.Join("-", new List<string?> { "a", null, "b" }).Should().Be("a--b");
        string.Join(", ", new List<int> { }).Should().Be("");
    }
}
