using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class TaskTests
{
    [Fact]
    public void IsCompletedSuccessfully_Test()
    {
        // Arrange
        var tcs1 = new TaskCompletionSource<int>();
        var tcs2 = new TaskCompletionSource<int>();

        tcs1.SetResult(10);
        tcs2.SetException(new Exception());

        // Act & assert
        tcs1.Task.IsCompletedSuccessfully.Should().BeTrue();
        tcs2.Task.IsCompletedSuccessfully.Should().BeFalse();
    }
}
