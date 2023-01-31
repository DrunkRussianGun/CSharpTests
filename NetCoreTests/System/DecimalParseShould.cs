using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;

namespace NetCoreTests.System;

public class DecimalParseShould
{
	[TestCaseSource(nameof(OnTryParse_WhenInvariantCulture_ReturnExpected_TestCaseSource))]
	public void OnTryParse_WhenInvariantCulture_ReturnExpected(string input, decimal expected)
	{
		var isSuccess = decimal.TryParse(
			input,
			NumberStyles.Float,
			NumberFormatInfo.InvariantInfo,
			out var actual);

		isSuccess.Should().BeTrue();
		actual.Should().Be(expected);
	}

	public static IEnumerable<TestCaseData> OnTryParse_WhenInvariantCulture_ReturnExpected_TestCaseSource()
	{
		yield return new TestCaseData("1.1", 1.1m);
		yield return new TestCaseData("01.10", 1.1m);
		yield return new TestCaseData("010.010", 10.01m);
	}

	[TestCaseSource(nameof(OnTryParse_WhenRuCulture_ReturnExpected_TestCaseSource))]
	public void OnTryParse_WhenRuCulture_ReturnExpected(string input, decimal expected)
	{
		var isSuccess = decimal.TryParse(
			input,
			NumberStyles.Float,
			CultureInfo.GetCultureInfo("ru-RU"),
			out var actual);

		isSuccess.Should().BeTrue();
		actual.Should().Be(expected);
	}

	public static IEnumerable<TestCaseData> OnTryParse_WhenRuCulture_ReturnExpected_TestCaseSource()
	{
		yield return new TestCaseData("1,1", 1.1m);
		yield return new TestCaseData("01,10", 1.1m);
		yield return new TestCaseData("010,010", 10.01m);
	}
}
