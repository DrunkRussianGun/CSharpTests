using FluentAssertions;

namespace NetCoreTests.System;

[Parallelizable(ParallelScope.All)]
public class StringShould
{
	public class OnSplit : StringShould
	{
		[Test]
		public void WhenEmpty_ResultInEmptySubstring()
		{
			var substrings = string.Empty.Split('0');

			substrings.Should().BeEquivalentTo(string.Empty);
		}
	}
}
