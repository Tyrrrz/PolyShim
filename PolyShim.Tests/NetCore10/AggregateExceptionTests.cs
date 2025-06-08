using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class AggregateExceptionTests
{
    [Fact]
    public void Throw_Test()
    {
        // Arrange
        var exception = new AggregateException(
            new Exception("Exception 1"),
            new Exception("Exception 2"),
            new Exception("Exception 3")
        );

        // Act
        var result = Assert.Throws<AggregateException>(void () => throw exception);

        // Assert
        result.Message.Should().Contain("One or more errors occurred");
        result.InnerExceptions[0].Message.Should().Be("Exception 1");
        result.InnerExceptions[1].Message.Should().Be("Exception 2");
        result.InnerExceptions[2].Message.Should().Be("Exception 3");
    }

    [Fact]
    public void Flatten_Test()
    {
        // Arrange
        var exception = new AggregateException(
            new AggregateException(new Exception("Exception 1"), new Exception("Exception 2")),
            new Exception("Exception 3")
        );

        // Act
        var result = exception.Flatten();

        // Assert
        result
            .InnerExceptions.Select(e => e.Message)
            .Should()
            .BeEquivalentTo("Exception 1", "Exception 2", "Exception 3");
    }

    [Fact]
    public void Handle_Test()
    {
        // Arrange
        var exception = new AggregateException(
            new Exception("Exception 1"),
            new Exception("Exception 2"),
            new Exception("Exception 3")
        );

        // Act
        var result = Assert.Throws<AggregateException>(void () =>
            exception.Handle(ex => ex.Message == "Exception 2")
        );

        // Assert
        result
            .InnerExceptions.Select(e => e.Message)
            .Should()
            .BeEquivalentTo("Exception 1", "Exception 3");
    }
}
