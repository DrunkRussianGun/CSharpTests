using FluentAssertions;

namespace NetCoreTests.System;

[Parallelizable(ParallelScope.All)]
public class DecimalShould
{
	[Test]
	public void WhenNull_NotEqualToZero()
	{
		var value = default(decimal?);

		var checkResult = value == 0;

		checkResult.Should().BeFalse();
	}

	[Test]
	public void WhenNullableZero_EqualToZero()
	{
		var value = (decimal?)0;

		var checkResult = value == 0;

		checkResult.Should().BeTrue();
	}
}
