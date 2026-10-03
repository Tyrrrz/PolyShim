using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class GCTests
{
    [Fact]
    public void AllocateUninitializedArray_Test()
    {
        // Act
        var array = GC.AllocateUninitializedArray<int>(16);

        // Assert
        array.Should().HaveCount(16);
    }

    [Fact]
    public void AllocateUninitializedArray_Pinned_Test()
    {
        // Act
        var array = GC.AllocateUninitializedArray<int>(16, true);

        // Assert
        array.Should().HaveCount(16);
    }
}
