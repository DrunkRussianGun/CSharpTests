using FluentAssertions;

namespace NetCoreTests;

[Parallelizable(ParallelScope.All)]
public class TypeOfShould
{
	[Test]
	public void GivenGenericType_ReturnNameOfActualType()
	{
		var typeName = TypeOf<Guid>().Name;

		typeName.Should().Be("Guid");
	}

	private static Type TypeOf<T>() => typeof(T);
}
