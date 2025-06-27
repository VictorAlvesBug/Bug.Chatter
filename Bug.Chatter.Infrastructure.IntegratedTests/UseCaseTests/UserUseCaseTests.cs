using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		private readonly Mock<IUserContext> _mockUserContext;

		private readonly IServiceProvider _scopeProvider;

		public UserUseCaseTests()
		{
			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_mockUserContext = new Mock<IUserContext>();
			OverrideWithMockUserContext(services, _mockUserContext);

			var _rootProvider = services.BuildServiceProvider(validateScopes: true);
			_scopeProvider = _rootProvider.CreateScope().ServiceProvider;
		}

		[SetUp]
		public void Setup()
		{
			Mock.Get(_mockUserContext.Object).Invocations.Clear();
		}

		private ServiceCollection OverrideWithMockUserContext(ServiceCollection services, Mock<IUserContext> mockUserContext)
		{
			UserDTO[] mockedUsers = [
				new(
					pk: "user-094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					sk: "user-mainSchema-v0",
					id: "094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					name: "Victor Bugueno",
					phoneNumber: "+55 (11) 97562-3736",
					version: 999,
					createdAt: "2025-06-27T00:00:00"),
				new(
					pk: "user-ea9983c8-be00-4307-93ad-635d961de718",
					sk: "user-mainSchema-v0",
					id: "ea9983c8-be00-4307-93ad-635d961de718",
					name: "Fatima Alves",
					phoneNumber: "+55 (11) 98237-5687",
					version: 999,
					createdAt: "2025-06-27T00:00:00")
			];

			mockUserContext
				.Setup(mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string pk, string sk, List<string> _) =>
					mockedUsers.FirstOrDefault(user => user.PK == pk && user.SK == sk)
				);

			mockUserContext
				.Setup(mock => mock.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string indexName, string indexPkValue, string indexSkValue, List<string> _) =>
				{
					var parts = indexName.Split('-');
					var indexPkName = parts[0];
					var indexSkName = parts[1];

					return mockedUsers.Where(user =>
					{
						var userIndexPkValue = user.GetType().GetProperty(indexPkName)?.GetValue(user)?.ToString();
						var userIndexSkValue = user.GetType().GetProperty(indexSkName)?.GetValue(user)?.ToString();

						if (indexSkValue is null)
							return userIndexPkValue == indexPkValue;

						return userIndexPkValue == indexPkValue && userIndexSkValue == indexSkValue;
					});
				});

			mockUserContext
				.Setup(mock => mock.BatchGetAsync(It.IsAny<IEnumerable<(string, string)>>(), It.IsAny<List<string>>()))
				.ReturnsAsync((IEnumerable<(string, string)> keysList, List<string> _) => 
					mockedUsers.Where(user => 
						keysList.Any(keys => keys.Item1 == user.PK && keys.Item2 == user.SK)
					)
				);

			mockUserContext
				.Setup(mock => mock.ListByPartitionKeyAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string pk, List<string> _) =>
					mockedUsers.Where(user => user.PK == pk)
				);

			mockUserContext
				.Setup(mock => mock.SafePutAsync(It.IsAny<UserDTO>()));

			mockUserContext
				.Setup(mock => mock.UpdateDynamicAsync(It.IsAny<UserDTO>()));

			mockUserContext
				.Setup(mock => mock.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()));


			services.AddScoped(_ => mockUserContext.Object);

			return services;
		}
	}
}