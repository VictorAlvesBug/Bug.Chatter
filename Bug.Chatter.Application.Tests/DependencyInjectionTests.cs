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
			foreach (var service in services.Where(s => s.ImplementationType is not null))
			{
				try
				{
					using var scope = provider.CreateScope();
					ActivatorUtilities.CreateInstance(scope.ServiceProvider, service.ImplementationType);
				}
				catch (Exception ex)
				{
					errorsSet.Add(ex.Message);
				}
			}

			// Assert
			Assert.That(errorsSet, Is.Empty, string.Join("\n", errorsSet));
		}
	}
}