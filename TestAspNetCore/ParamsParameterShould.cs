using FluentAssertions;
using NUnit.Framework;

namespace TestNetCore
{
    [TestFixture]
    public class ParamsParameterShould
    {
        [Test]
        public void WhenNotSpecified_BeEmpty()
        {
            var parameter = GetParamsParameter<int>();

            parameter.Should().BeEmpty();
        }

        private static T[] GetParamsParameter<T>(params T[] parameter) => parameter;
    }
}
