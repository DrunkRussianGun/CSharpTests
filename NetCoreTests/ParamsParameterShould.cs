using FluentAssertions;

namespace NetCoreTests;

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