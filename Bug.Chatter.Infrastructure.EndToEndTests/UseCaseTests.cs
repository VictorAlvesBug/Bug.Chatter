using Bug.Chatter.Application.Users.InitializeUser;
using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Infrastructure.IntegratedTests
{
	internal class UseCaseTests
	{
		private ServiceProvider? _rootProvider;

		[SetUp]
		public void Setup()
		{
			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_rootProvider = services.BuildServiceProvider(validateScopes: true);
		}

		[TearDown]
		public void TearDown()
		{
			_rootProvider?.Dispose();
		}

		[Test]
		public async Task HandleAsync_InitializeUserUseCase_EndToEndTest()
		{
			// Arrange
			var initializeUserUseCase = CreateScopeProvider().GetRequiredService<InitializeUserUseCase>();

			var command = new InitializeUserCommand("Victor Bugueno", "+55 (11) 97562-3736");

			// Act
			var result = await initializeUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});
		}

		private IServiceProvider CreateScopeProvider() => _rootProvider.CreateScope().ServiceProvider;
	}
}
