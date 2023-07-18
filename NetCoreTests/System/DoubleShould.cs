using System.Globalization;
using FluentAssertions;

namespace NetCoreTests.System;

[Parallelizable(ParallelScope.All)]
public class DoubleShould
{
	[TestCase(61.833333333333333333333d, "61.833333333333336")]
	public void BeRepresentedInaccurately(double value, string expected)
	{
		var actual = value.ToString(CultureInfo.InvariantCulture);

		actual.Should().Be(expected);
	}
}
