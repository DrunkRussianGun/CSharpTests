using FluentAssertions;

namespace NetCoreTests;

[TestFixture]
public class ConversionToStringShould
{
    [Test]
    public void WhenDecimalWithoutFraction_ReturnStringWithoutFraction()
    {
        const decimal number = 1m;

        var actualString = number.ToString();

        actualString.Should().NotContainAny(".", ",");
    }

    [Test]
    public void WhenJoiningEmptyStrings_ReturnSeparator()
    {
        var actualString = string.Join(",", string.Empty, string.Empty);

        actualString.Should().Be(",");
    }

    [TestCase("\u1160", "ᅠ")]
    public void WhenUnicodeEscape_ReturnExpectedString(string actual, string expected)
        => actual.Should().Be(expected);
}
