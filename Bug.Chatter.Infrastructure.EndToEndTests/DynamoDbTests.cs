using Bug.Chatter.Application.Aggregates.Users.ValidateNewUser;
using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.DependencyInjection;
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
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			var userDto = new UserDTO(
				"user-2a902140-6191-4f04-9176-6fb66753cf6a",
				//"user-bd9ec526-1d38-478e-a873-596086bca4ab",
				"user-mainSchema-v0",
				null,
				"Outro User",
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
			/*var sample = "10/01/1999 - 01:05";
			var format = "dd/MM/yyyy - HH:mm";
			var date1 = DateTime.ParseExact(sample, format, CultureInfo.CurrentCulture);
			var date2 = DateTime.ParseExact(sample, format, CultureInfo.InvariantCulture);

			var date3 = date1.ToUniversalTime();
			var date4 = date2.ToUniversalTime();

			Console.WriteLine("");*/

			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			var userDto = new UserDTO(
				"user-2a902140-6191-4f04-9176-6fb66753cf6a",
				"user-mainSchema-v0",
				null,
				"2 Nome Atualizado",
				null,
				5,
				DateTime.UtcNow.ToBrazilianStringDateTime());

			// Act
			await userContext.UpdateDynamicAsync(userDto);

			var result = await userContext.GetAsync(userDto.PK, userDto.SK);

			TestContext.WriteLine(result.ToJson());
		}

		[Test]
		public async Task DynamoDb_ListByIndexKeysAsync_EndToEndTest()
		{
			// Arrange
			var userContext = CreateScopeProvider().GetRequiredService<IUserContext>();

			// Act
			var result = await userContext.ListByIndexKeysAsync("PhoneNumber-SK-index", "+55 (11) 97562-3736");

			TestContext.WriteLine(result.ToJson());
		}

		private IServiceProvider CreateScopeProvider() => _rootProvider.CreateScope().ServiceProvider;
	}
}
