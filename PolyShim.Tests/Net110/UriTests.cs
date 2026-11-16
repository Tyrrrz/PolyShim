using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class UriTests
{
    [Fact]
    public void UriSchemeData_Test()
    {
        // Act & assert
        Uri.UriSchemeData.Should().Be("data");
    }
}
