using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.IntegratedTests.SeedWork;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		private readonly DatabaseMock _databaseMock;

		private readonly Mock<IDynamoDbRepository<UserDTO>> _mockUserContext;

		private readonly IServiceProvider _scopeProvider;

		public UserUseCaseTests()
		{
			_databaseMock = new();

			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_mockUserContext = new Mock<IDynamoDbRepository<UserDTO>>();
			services.OverrideWithMockContext<IDynamoDbRepository<UserDTO>, UserDTO>(_databaseMock, _mockUserContext);

			var _rootProvider = services.BuildServiceProvider(validateScopes: true);
			_scopeProvider = _rootProvider.CreateScope().ServiceProvider;
		}

		[SetUp]
		public void Setup()
		{
			_databaseMock.UseDefaultUsers();
			Mock.Get(_mockUserContext.Object).Invocations.Clear();
		}
	}
}