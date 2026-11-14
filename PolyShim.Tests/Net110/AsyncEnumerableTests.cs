using System.Linq;
using System.Threading.Tasks;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class AsyncEnumerableTests
{
    [Fact]
    public async Task FullJoin_Test()
    {
        var first = new[] { 1, 2, 3 }.ToAsyncEnumerable();
        var second = new[] { 3, 4 }.ToAsyncEnumerable();

        var result = await first
            .FullJoin(second, x => x, x => x, (a, b) => $"{a}-{b}")
            .ToListAsync();

        result.Should().Equal("1-0", "2-0", "3-3", "0-4");
    }
}
