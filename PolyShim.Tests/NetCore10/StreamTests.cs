using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class StreamTests
{
    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        using var source = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        using var destination = new MemoryStream();

        // Act
        source.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }
}