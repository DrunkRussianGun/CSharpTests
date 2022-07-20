using FluentAssertions;

namespace NetCoreTests.NUnit.Framework;

[TestFixture]
public class CollectionAssertShould
{
    private class ValueContainer
    {
        public int Value { get; set; }

        public override bool Equals(object? other)
            => other != null && GetType() == other.GetType() && Equals((ValueContainer)other);

        public bool Equals(ValueContainer? other)
            => other != null && Value == other.Value;
    }

    [Test]
    public void OnAreEqual_WhenCollectionsContainDifferentReferences_Pass()
    {
        var first = new[] { new ValueContainer() };
        var second = new[] { new ValueContainer() };

        Action areEqualCall = () => CollectionAssert.AreEqual(second, first);
            
        areEqualCall.Should().NotThrow();
    }

    [Test]
    public void OnAreEquivalent_WhenCollectionsContainDifferentReferences_Pass()
    {
        var first = new[] { new ValueContainer() };
        var second = new[] { new ValueContainer() };

        Action areEquivalentCall = () => CollectionAssert.AreEquivalent(second, first);
            
        areEquivalentCall.Should().NotThrow();
    }
}