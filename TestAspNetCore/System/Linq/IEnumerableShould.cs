using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TestNetCore.System.Linq
{
	[TestFixture]
	public class IEnumerableShould
	{
		[Test]
		public void WithoutToArrayCall_BeEvaluatedLazily()
		{
			bool isEvaluated = false;

			Enumerable
				.Range(1, 1)
				.Select(
					_ =>
					{
						isEvaluated = true;
						return 1;
					})
				.Select(x =>
				{
					isEvaluated.Should().BeTrue();
					return (Func<int>)(() => x);
				})
				.ToArray();
		}
	}
}