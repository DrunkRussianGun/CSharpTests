using FluentAssertions;

namespace NetCoreTests;

[TestFixture]
public class FluentAssertionsShould
{
    public class OnBeEquivalentTo : FluentAssertionsShould
    {
        [Test]
        public void WhenEnumerablesAreStrictlyEquivalent_Pass()
        {
            var first = new[] { 0, 1 };
            var second = new[] { 0, 1 };

            first
                .Invoking(x => x.Should().BeEquivalentTo(second))
                .Should().NotThrow();
        }

        [Test]
        public void WhenEnumerablesContainSameElements_Pass()
        {
            var first = new[] { 0, 1 };
            var second = new[] { 1, 0 };

            first
                .Invoking(x => x.Should().BeEquivalentTo(second))
                .Should().NotThrow();
        }
    }

    public class OnEqual : FluentAssertionsShould
    {
        [Test]
        public void WhenEnumerablesContainSameElements_Throw()
        {
            var first = new[] { 0, 1 };
            var second = new[] { 1, 0 };

            first
                .Invoking(x => x.Should().Equal(second))
                .Should().Throw<Exception>();
        }
    }

    public class OnContainSingle : FluentAssertionsShould
    {
        [Test]
        public void WhenCollectionContainsOtherNonMatchingItems_Pass()
        {
            var collection = new[] { 1, 2 };

            collection
                .Invoking(x => x.Should().ContainSingle(number => number == 1))
                .Should().NotThrow();
        }
        [Test]
        public void WhenCollectionContainsSeveralMatchingItemsAndOtherNonMatchingItems_Throw()
        {
            var collection = new[] { 1, 2, 1 };

            collection
                .Invoking(x => x.Should().ContainSingle(number => number == 1))
                .Should().Throw<AssertionException>();
        }
    }
}