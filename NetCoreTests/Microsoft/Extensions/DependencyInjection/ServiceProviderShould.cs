using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreTests.Microsoft.Extensions.DependencyInjection.Helpers;
using NUnit.Framework;

namespace NetCoreTests.Microsoft.Extensions.DependencyInjection
{
	[TestFixture]
	public class ServiceProviderShould
	{
		[Test]
		public void NotAutoRegisterNestedInterface()
		{
			var serviceProvider = CreateServiceProvider((_, _) => { });

			var actual = serviceProvider.GetService<INestedInterface>();

			actual.Should().BeNull();
		}

		[Test]
		public void BeAbleToExplicitlyRegisterNestedInterface()
		{
			var serviceProvider = CreateServiceProvider(
				(_, services) => services.AddTransient<INestedInterface, NestedClass>());

			var actual = serviceProvider.GetService<INestedInterface>();

			actual.Should().NotBeNull();
		}
		
		[Test]
		public void NotAutoRegisterInterface()
		{
			var serviceProvider = CreateServiceProvider((_, _) => { });

			var actual = serviceProvider.GetService<IInterface>();

			actual.Should().BeNull();
		}

		[Test]
		public void BeAbleToRegisterInterface()
		{
			var serviceProvider = CreateServiceProvider(
				(_, services) => services.AddTransient<IInterface>());

			var actual = serviceProvider.GetService<IInterface>();

			actual.Should().NotBeNull();
		}
		
		public interface INestedInterface { }
		
		public class NestedClass : INestedInterface { }

		private static IServiceProvider CreateServiceProvider(
			Action<HostBuilderContext, IServiceCollection> serviceCollectionConfig)
			=> Host.CreateDefaultBuilder()
				.ConfigureServices(serviceCollectionConfig)
				.Build()
				.Services;
	}
}