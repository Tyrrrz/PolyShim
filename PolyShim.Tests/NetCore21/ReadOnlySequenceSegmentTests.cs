using System;
using System.Buffers;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class ReadOnlySequenceSegmentTests
{
    [Fact]
    public void Memory_Test()
    {
        // Arrange
        var segment = new TestSegment(new byte[] { 1, 2, 3 });

        // Assert
        segment.Memory.ToArray().Should().Equal(1, 2, 3);
    }

    [Fact]
    public void NextAndRunningIndex_Test()
    {
        // Arrange
        var first = new TestSegment(new byte[] { 1, 2 });
        var second = new TestSegment(new byte[] { 3, 4, 5 });

        // Act
        first.LinkTo(second);

        // Assert
        first.Next.Should().BeSameAs(second);
        second.RunningIndex.Should().Be(2);
    }

    private sealed class TestSegment : ReadOnlySequenceSegment<byte>
    {
        public TestSegment(ReadOnlyMemory<byte> memory)
        {
            Memory = memory;
        }

        public void LinkTo(TestSegment next)
        {
            next.RunningIndex = RunningIndex + Memory.Length;
            Next = next;
        }
    }
}
