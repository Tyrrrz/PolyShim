using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class InterlockedTests
{
    private enum IntEnum : int
    {
        Value1 = 0x0000FFFF,
        Value2 = unchecked((int)0xFFFF0000),
    }

    [Fact]
    public void And_SupportsVariousIntegerTypes()
    {
        // uint
        uint ui = 0xFFFFFFFF;
        Interlocked.And(ref ui, 0xAAAAAAAA).Should().Be(0xFFFFFFFF);
        ui.Should().Be(0xAAAAAAAA);

        // int
        int i = unchecked((int)0xFFFFFFFF);
        Interlocked.And(ref i, unchecked((int)0xAAAAAAAA)).Should().Be(unchecked((int)0xFFFFFFFF));
        i.Should().Be(unchecked((int)0xAAAAAAAA));

        // ulong
        ulong ul = 0xFFFFFFFFFFFFFFFF;
        Interlocked.And(ref ul, 0xAAAAAAAAAAAAAAAA).Should().Be(0xFFFFFFFFFFFFFFFF);
        ul.Should().Be(0xAAAAAAAAAAAAAAAA);

        // long
        long l = unchecked((long)0xFFFFFFFFFFFFFFFF);
        Interlocked
            .And(ref l, unchecked((long)0xAAAAAAAAAAAAAAAA))
            .Should()
            .Be(unchecked((long)0xFFFFFFFFFFFFFFFF));
        l.Should().Be(unchecked((long)0xAAAAAAAAAAAAAAAA));

        // enum
        var e = IntEnum.Value1;
        Interlocked.And(ref e, IntEnum.Value2).Should().Be(IntEnum.Value1);
        e.Should().Be((IntEnum)0);
    }

    [Fact]
    public void Or_SupportsVariousIntegerTypes()
    {
        // uint
        uint ui = 0x0000FFFF;
        Interlocked.Or(ref ui, 0xFFFF0000).Should().Be(0x0000FFFF);
        ui.Should().Be(0xFFFFFFFF);

        // int
        int i = 0x0000FFFF;
        Interlocked.Or(ref i, unchecked((int)0xFFFF0000)).Should().Be(0x0000FFFF);
        i.Should().Be(unchecked((int)0xFFFFFFFF));

        // ulong
        ulong ul = 0x00000000FFFFFFFF;
        Interlocked.Or(ref ul, 0xFFFFFFFF00000000).Should().Be(0x00000000FFFFFFFF);
        ul.Should().Be(0xFFFFFFFFFFFFFFFF);

        // long
        long l = 0x00000000FFFFFFFF;
        Interlocked.Or(ref l, unchecked((long)0xFFFFFFFF00000000)).Should().Be(0x00000000FFFFFFFF);
        l.Should().Be(unchecked((long)0xFFFFFFFFFFFFFFFF));

        // enum
        var e = IntEnum.Value1;
        Interlocked.Or(ref e, IntEnum.Value2).Should().Be(IntEnum.Value1);
        e.Should().Be((IntEnum)unchecked((int)0xFFFFFFFF));
    }

    [Fact]
    public void And_ThrowsForFloatingPointTypes()
    {
        float f = 1.5f;
        var ex = Assert.Throws<NotSupportedException>(() => Interlocked.And(ref f, 2.5f));
        ex.Message.Should().Contain("integer");

        double d = 1.5;
        ex = Assert.Throws<NotSupportedException>(() => Interlocked.And(ref d, 2.5));
        ex.Message.Should().Contain("integer");
    }

    [Fact]
    public void Or_ThrowsForFloatingPointTypes()
    {
        float f = 1.5f;
        var ex = Assert.Throws<NotSupportedException>(() => Interlocked.Or(ref f, 2.5f));
        ex.Message.Should().Contain("integer");

        double d = 1.5;
        ex = Assert.Throws<NotSupportedException>(() => Interlocked.Or(ref d, 2.5));
        ex.Message.Should().Contain("integer");
    }
}
