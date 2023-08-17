using System.Linq;
using FluentAssertions;

namespace NetCoreTests.System.Linq;

[TestFixture]
public class IEnumerableShould
{
	[Test]
	public void WithoutToArrayCall_BeEvaluatedLazily()
	{
		bool isEvaluated = false;

		var lazySequence = Enumerable
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
			});
		
		isEvaluated.Should().BeFalse();
			
		_ = lazySequence.ToArray();

		isEvaluated.Should().BeTrue();
	}

	[Test]
	public void ExceptByProduceUniqueElements()
	{
		var sequence = new[] { 5, 2, 6, 5, 1 }
			.Select(x => new { Value = x })
			.ToArray();
		var expected = sequence.Take(3).Append(sequence.Last()).ToArray();

		var actual = sequence.ExceptBy(new[] { 0 }, x => x.Value).ToArray();

		actual.Should().Equal(expected);
	}
}
