#nullable enable
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreTests.Microsoft.Extensions.DependencyInjection.Helpers;

namespace NetCoreTests.Microsoft.Extensions.DependencyInjection;

[Parallelizable(ParallelScope.All)]
public class ServiceProviderShould
{
	[Test]
	public void NotAutoRegisterNestedInterface()
	{
		var serviceProvider = CreateServiceProvider(_ => { });

		var actual = serviceProvider.GetService<INestedInterface>();

		actual.Should().BeNull();
	}

	[Test]
	public void BeAbleToExplicitlyRegisterNestedInterface()
	{
		var serviceProvider = CreateServiceProvider(
			services => services.AddTransient<INestedInterface, NestedClass>());

		var actual = serviceProvider.GetService<INestedInterface>();

		actual.Should().NotBeNull();
	}
		
	[Test]
	public void NotAutoRegisterInterface()
	{
		var serviceProvider = CreateServiceProvider(_ => { });

		var actual = serviceProvider.GetService<IInterface>();

		actual.Should().BeNull();
	}

	[Test]
	public void BeAbleToRegisterInterface()
	{
		var serviceProvider = CreateServiceProvider(
			services => services.AddTransient<IInterface>());

		var actual = serviceProvider.GetService<IInterface>();

		actual.Should().NotBeNull();
	}
		
	public interface INestedInterface { }
		
	public class NestedClass : INestedInterface { }

	private static IServiceProvider CreateServiceProvider(
		Action<IServiceCollection>? serviceCollectionConfig = null)
	{
		var serviceCollection = new ServiceCollection();
		serviceCollectionConfig?.Invoke(serviceCollection);
		return serviceCollection.BuildServiceProvider();
	}
}
