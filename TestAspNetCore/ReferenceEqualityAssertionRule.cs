using FluentAssertions;
using FluentAssertions.Equivalency;

namespace TestNetCore
{
    public class ReferenceEqualityAssertionRule : IAssertionRule
    {
        public bool AssertEquality(IEquivalencyValidationContext context)
        {
            context.Subject.Should().BeSameAs(context.Expectation, context.Because, context.BecauseArgs);
            return true;
        }
    }
}
