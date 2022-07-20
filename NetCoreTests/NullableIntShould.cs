using FluentAssertions;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace NetCoreTests;

[TestFixture]
public class NullableIntShould
{
    [Test]
    public void NotBeLessThan0()
    {
        (null < 0).Should().BeFalse();
    }

    [Test]
    public void NotBeGreaterThan0()
    {
        (null > 0).Should().BeFalse();
    }
}