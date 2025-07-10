using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Application.Tests
{
	public class DependencyInjectionTests
	{
		[Test]
		public void ShouldValidateAllDependencies()
		{
			// Arrange
			var services = new ServiceCollection();

			services.AddApplicationServices();
			services.AddInfrastructureServices();

			var provider = services.BuildServiceProvider(validateScopes: true);
			var errorsSet = new HashSet<string>();

			// Act
			foreach (var service in services)
			{
				if (service.ServiceType.IsGenericType && service.ServiceType.ContainsGenericParameters)
					continue;

				try
				{
					using var scope = provider.CreateScope();

					if (service.ImplementationType != null)
					{
						ActivatorUtilities.CreateInstance(scope.ServiceProvider, service.ImplementationType);
					}
					else if (service.ImplementationFactory != null)
					{
						service.ImplementationFactory(scope.ServiceProvider);
					}
				}
				catch (Exception ex)
				{
					errorsSet.Add($"Service: {service.ServiceType.FullName}\nError: {ex.Message}");
				}
			}

			// Assert
			Assert.That(errorsSet, Is.Empty, string.Join("\n\n", errorsSet));
		}
	}
}