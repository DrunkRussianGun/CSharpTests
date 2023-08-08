using FluentAssertions;

namespace NetCoreTests;

public class NameOfShould
{
	[Test]
	public void ForNamespace_ReturnTheLastPart()
	{
		var nameOfNamespace = nameof(NUnit.Framework);

		nameOfNamespace.Should().Be("Framework");
	}

	[Test]
	public void ForGeneric_ReturnNameOfGenericType()
	{
		var actualName = NameOf<Guid>();

		actualName.Should().Be("T");
	}

	private static string NameOf<T>() => nameof(T);
}
