using FluentAssertions;

namespace NetCoreTests.System;

[TestFixture]
public class NullableShould
{
	[Test]
	public void WhenHasNoValue_ReturnEmptyString()
	{
		int? nullInt = null;

		var actualString = nullInt.ToString();

		actualString.Should().BeEmpty();
	}
}