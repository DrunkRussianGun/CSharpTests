using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace NetCoreTests.NUnit.Framework;

[Parallelizable]
public class TestCaseSourceShould
{
	private List<int> seenValues;

	[OneTimeSetUp]
	public void OneTimeSetUp()
	{
		seenValues = new List<int>();
	}

	[OneTimeTearDown]
	public void OneTimeTearDown()
	{
		seenValues.Should().Equal(Enumerable.Range(1, 4));
	}
		
	[TestCaseSource(nameof(From1To2))]
	[TestCaseSource(nameof(From3To4))]
	public void ConcatenateTwoSources(int value)
	{
		seenValues.Add(value);
	}

	public static IEnumerable<TestCaseData> From1To2()
		=> Enumerable.Range(1, 2).Select(x => new TestCaseData(x));

	public static IEnumerable<TestCaseData> From3To4()
		=> Enumerable.Range(3, 2).Select(x => new TestCaseData(x));
}