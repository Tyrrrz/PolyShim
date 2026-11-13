using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class ReadOnlySequenceStreamTests
{
    [Fact]
    public void Read_Test()
    {
        // Arrange
        var source = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
        using var stream = new ReadOnlySequenceStream(source);
        var destination = new byte[5];

        // Act
        var bytesRead = stream.Read(destination, 0, destination.Length);

        // Assert
        bytesRead.Should().Be(5);
        destination.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Read_MultiSegment_Test()
    {
        // Arrange
        var source = CreateSequence([1, 2], [3, 4, 5]);
        using var stream = new ReadOnlySequenceStream(source);
        var destination = new byte[5];

        // Act
        var bytesRead = stream.Read(destination, 0, destination.Length);

        // Assert
        bytesRead.Should().Be(5);
        destination.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Read_Empty_Test()
    {
        // Arrange
        using var stream = new ReadOnlySequenceStream(ReadOnlySequence<byte>.Empty);
        var result = new byte[8];

        // Act
        var bytesRead = stream.Read(result, 0, result.Length);

        // Assert
        bytesRead.Should().Be(0);
        stream.ReadByte().Should().Be(-1);
    }

    [Fact]
    public void Write_NotSupported_Test()
    {
        // Arrange
        using var stream = new ReadOnlySequenceStream(
            new ReadOnlySequence<byte>(new byte[] { 1, 2, 3 })
        );

        // Act
        var writeAct = () => stream.Write(new byte[1], 0, 1);
        var setLengthAct = () => stream.SetLength(1);

        // Assert
        writeAct.Should().Throw<NotSupportedException>();
        setLengthAct.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        var stream = new ReadOnlySequenceStream(new ReadOnlySequence<byte>(new byte[] { 1, 2, 3 }));

        // Act
        stream.Dispose();

        // Assert
        stream.CanRead.Should().BeFalse();
        stream.CanSeek.Should().BeFalse();

        var readAct = () => stream.ReadByte();
        readAct.Should().Throw<ObjectDisposedException>();
    }

    private static ReadOnlySequence<byte> CreateSequence(params byte[][] segments)
    {
        if (segments.Length == 0)
            return ReadOnlySequence<byte>.Empty;

        BufferSegment? first = null;
        BufferSegment? current = null;
        foreach (var segment in segments)
        {
            var next = new BufferSegment(segment);
            if (first is null)
            {
                first = current = next;
                continue;
            }

            current = current!.Append(next);
        }

        return new ReadOnlySequence<byte>(first!, 0, current!, current!.Memory.Length);
    }

    private sealed class BufferSegment : ReadOnlySequenceSegment<byte>
    {
        public BufferSegment(ReadOnlyMemory<byte> memory) => Memory = memory;

        public BufferSegment Append(BufferSegment next)
        {
            next.RunningIndex = RunningIndex + Memory.Length;
            Next = next;
            return next;
        }
    }
}
