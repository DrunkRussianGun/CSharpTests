using System.Runtime.CompilerServices;
using FluentAssertions;

namespace NetCoreTests;

public class SwitchExpressionShould
{
	[Test]
	public void SpecifyValueInSwitchExpressionException()
	{
		Func<int> nonExhaustiveSwitch = () => 42 switch { };

		nonExhaustiveSwitch.Should().Throw<SwitchExpressionException>().WithMessage("*42*");
	}
}
