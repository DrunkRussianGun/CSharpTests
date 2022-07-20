using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;

namespace NetCoreTests.System.Expressions
{
    [TestFixture]
    public class ExpressionShould
    {
        [Test]
        public void NotInlineVariableValues()
        {
            var variable = "Cool string";

            Expression<Func<string>> expression = () => variable;

            expression.Body.NodeType.Should().Be(ExpressionType.MemberAccess);
            expression.ToString().Should().NotContain(variable);
        }

        [Test]
        public void BeAbleToGetVariableValue()
        {
            var variable = "Variable value";

            Expression<Func<string>> expression = () => variable;

            Evaluate(expression).Should().Be(variable);
        }

        private static T Evaluate<T>(Expression<Func<T>> expression)
            => expression.Compile().Invoke();
    }
}
