using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Infrastructure.IntegratedTests
{
	internal class DynamoDbTests
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
		public async Task DynamoDb_SafePutAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			var userDto = new UserDTO(
				"user-2a902140-6191-4f04-9176-6fb66753cf6a",
				"user-mainSchema-v0",
				null,
				"Outro User",
				null,
				null,
				5,
				DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));

			// Act
			await userContext.SafePutAsync(userDto);
		}

		[Test]
		public async Task DynamoDb_GetAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			// Act
			var result = await userContext.GetAsync("user-2a902140-6191-4f04-9176-6fb66753cf6a", "user-mainSchema-v0");

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_ListByPartitionKeyAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			// Act
			var result = await userContext.ListByPartitionKeyAsync("user-2a902140-6191-4f04-9176-6fb66753cf6a");

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_BatchGetAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			// Act
			var result = await userContext.BatchGetAsync([
				("user-094b1c2d-ee50-4c68-a18a-8dca65d450c6", "user-mainSchema-v0"),
				("user-ea9983c8-be00-4307-93ad-635d961de718", "user-mainSchema-v0")
			]);

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_DeleteAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			// Act
			await userContext.DeleteAsync("user-094b1c2d-ee50-4c68-a18a-8dca65d450c6", "user-mainSchema-v0", 3);
		}

		[Test]
		public async Task DynamoDb_UpdateDynamicAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			var userDto = new UserDTO(
				"user-094b1c2d-ee50-4c68-a18a-8dca65d450c6",
				"user-mainSchema-v0",
				null,
				"2 Nome Atualizado",
				null,
				null,
				5,
				DateTime.UtcNow.ToBrazilianStringDateTime());

			// Act
			await userContext.UpdateDynamicAsync(userDto, 3);

			var result = await userContext.GetAsync(userDto.PK, userDto.SK);

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_ListByIndexKeysAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IDynamoDbRepository<UserDTO>>();

			// Act
			var result = await userContext.ListByIndexKeysAsync("PhoneNumber-SK-index", "+55 (11) 97562-3736");

			TestContext.WriteLine(result.ToJson());
		}

		private IServiceProvider CreateScopeProvider() => _rootProvider.CreateScope().ServiceProvider;
	}
}
