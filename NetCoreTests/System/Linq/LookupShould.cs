using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace NetCoreTests.System.Linq
{
    [TestFixture]
    public class LookupShould
    {
        [Test]
        public void WhenKeyDoesNotExists_ReturnEmptyCollection()
        {
            var lookup = new[] { 0 }.ToLookup(x => x);

            lookup[1].Should().BeEmpty();
        }

        [Test]
        public void AllowMultipleEnumeration()
        {
            using var enumerator = GetAlternating0And1().GetEnumerator();

            var alternatingNumbers = GetNextValues(enumerator, 3);
            // ReSharper disable PossibleMultipleEnumeration
            alternatingNumbers.Should().BeEquivalentTo(new[] { 0, 1, 0 });
            alternatingNumbers.Should().BeEquivalentTo(new[] { 1, 0, 1 });
            // ReSharper restore PossibleMultipleEnumeration

            var expectedLookup = new[] { 0, 1, 0 }.ToLookup(x => x);
            var actualLookup = GetNextValues(enumerator, 3).ToLookup(x => x);
            actualLookup.Should().BeEquivalentTo(expectedLookup);
            actualLookup.Should().BeEquivalentTo(expectedLookup);
        }

        private static IEnumerable<T> GetNextValues<T>(IEnumerator<T> enumerator, int count)
            => Enumerable.Range(0, count).Select(_ =>
            {
                enumerator.MoveNext();
                return enumerator.Current;
            });

        private static IEnumerable<int> GetAlternating0And1()
        {
            var isZero = true;
            while (true)
            {
                yield return isZero ? 0 : 1;
                isZero = !isZero;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
