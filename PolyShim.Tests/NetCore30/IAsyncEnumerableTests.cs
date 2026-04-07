using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class IAsyncEnumerableTests
{
    private class NumberSequence : IAsyncEnumerable<int>
    {
        private readonly int[] _numbers;

        public NumberSequence(int[] numbers) => _numbers = numbers;

        public IAsyncEnumerator<int> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        ) => new Enumerator(_numbers);

        private class Enumerator : IAsyncEnumerator<int>
        {
            private readonly int[] _numbers;
            private int _index = -1;

            public Enumerator(int[] numbers) => _numbers = numbers;

            public int Current => _numbers[_index];

            public ValueTask<bool> MoveNextAsync()
            {
                _index++;
                return new ValueTask<bool>(_index < _numbers.Length);
            }

            public ValueTask DisposeAsync() => default;
        }
    }

    [Fact]
    public async Task AwaitForEach_Test()
    {
        // Arrange
        var sequence = new NumberSequence([1, 2, 3, 4, 5]);
        var result = new List<int>();

        // Act
        await foreach (var item in sequence)
            result.Add(item);

        // Assert
        result.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task AwaitForEach_Empty_Test()
    {
        // Arrange
        var sequence = new NumberSequence([]);
        var result = new List<int>();

        // Act
        await foreach (var item in sequence)
            result.Add(item);

        // Assert
        result.Should().BeEmpty();
    }
}
