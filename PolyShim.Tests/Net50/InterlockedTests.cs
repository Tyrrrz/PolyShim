using System.Threading;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class InterlockedTests
{
    [Fact]
    public void And_Test()
    {
        // int
        var i = unchecked((int)0xFFFFFFFF);
        Interlocked.And(ref i, unchecked((int)0xAAAAAAAA)).Should().Be(unchecked((int)0xFFFFFFFF));
        i.Should().Be(unchecked((int)0xAAAAAAAA));

        // uint
        var ui = 0xFFFFFFFF;
        Interlocked.And(ref ui, 0xAAAAAAAA).Should().Be(0xFFFFFFFF);
        ui.Should().Be(0xAAAAAAAA);

        // long
        var l = unchecked((long)0xFFFFFFFFFFFFFFFF);
        Interlocked
            .And(ref l, unchecked((long)0xAAAAAAAAAAAAAAAA))
            .Should()
            .Be(unchecked((long)0xFFFFFFFFFFFFFFFF));
        l.Should().Be(unchecked((long)0xAAAAAAAAAAAAAAAA));

        // ulong
        var ul = 0xFFFFFFFFFFFFFFFF;
        Interlocked.And(ref ul, 0xAAAAAAAAAAAAAAAA).Should().Be(0xFFFFFFFFFFFFFFFF);
        ul.Should().Be(0xAAAAAAAAAAAAAAAA);
    }

    [Fact]
    public void Or_Test()
    {
        // int
        var i = 0x0000FFFF;
        Interlocked.Or(ref i, unchecked((int)0xFFFF0000)).Should().Be(0x0000FFFF);
        i.Should().Be(unchecked((int)0xFFFFFFFF));

        // uint
        uint ui = 0x0000FFFF;
        Interlocked.Or(ref ui, 0xFFFF0000).Should().Be(0x0000FFFF);
        ui.Should().Be(0xFFFFFFFF);

        // long
        long l = 0x00000000FFFFFFFF;
        Interlocked.Or(ref l, unchecked((long)0xFFFFFFFF00000000)).Should().Be(0x00000000FFFFFFFF);
        l.Should().Be(unchecked((long)0xFFFFFFFFFFFFFFFF));

        // ulong
        ulong ul = 0x00000000FFFFFFFF;
        Interlocked.Or(ref ul, 0xFFFFFFFF00000000).Should().Be(0x00000000FFFFFFFF);
        ul.Should().Be(0xFFFFFFFFFFFFFFFF);
    }
}
