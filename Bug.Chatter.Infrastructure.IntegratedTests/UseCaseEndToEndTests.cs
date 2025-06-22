using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users.CreateUser;
using Bug.Chatter.Domain.SeedWork.ValueObjects;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Infrastructure.IntegratedTests
{
	internal class UseCaseEndToEndTests
	{
		private ServiceProvider _rootProvider;

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
		public async Task HandleAsync_CreateUserUseCase_EndToEndTest()
		{
			// Arrange
			var createUserUseCase = CreateScopeProvider().GetRequiredService<CreateUserUseCase>();

			var command = new CreateUserCommand("Victor Bugueno", "+55 (11) 97562-3736");

			// Act
			var result = await createUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});
		}

		[Test]
		public async Task DynamoDb_SafePutAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			var userDto = new UserDTO(
				"user-2a902140-6191-4f04-9176-6fb66753cf6a",
				//"user-bd9ec526-1d38-478e-a873-596086bca4ab",
				"user-mainSchema-v0",
				null,
				"Outro User",
				null,
				5);

			// Act
			await userContext.SafePutAsync(userDto);
		}

		[Test]
		public async Task DynamoDb_GetAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			// Act
			var result = await userContext.GetAsync("user-2a902140-6191-4f04-9176-6fb66753cf6a", "user-mainSchema-v0");

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_ListByPartitionKeyAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			// Act
			var result = await userContext.ListByPartitionKeyAsync("user-2a902140-6191-4f04-9176-6fb66753cf6a");

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_BatchGetAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			// Act
			var result = await userContext.BatchGetAsync([
				("user-2a902140-6191-4f04-9176-6fb66753cf6a", "user-mainSchema-v0"),
				("user-c9ab6f85-fa95-4bd6-acc7-5d58cdbc211a", "user-mainSchema-v0")
			]);

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_DeleteAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			// Act
			await userContext.DeleteAsync("user-2a902140-6191-4f04-9176-6fb66753cf6a", "user-mainSchema-v0");
		}

		[Test]
		public async Task DynamoDb_UpdateDynamicAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			var userDto = new UserDTO(
				"user-2a902140-6191-4f04-9176-6fb66753cf6a",
				"user-mainSchema-v0",
				null,
				"Nome Atualizado",
				null,
				5);

			// Act
			var result = await userContext.UpdateDynamicAsync(userDto);

			TestContext.WriteLine(result.ToJson());
			// ERRO: System.ArgumentNullException : Value cannot be null. (Parameter 'obj')
		}

		private IServiceProvider CreateScopeProvider() => _rootProvider.CreateScope().ServiceProvider;
	}
}
