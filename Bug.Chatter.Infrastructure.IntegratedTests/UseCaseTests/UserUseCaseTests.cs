using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.IntegratedTests.SeedWork;
using Bug.Chatter.Infrastructure.IntegratedTests.SeedWork.InMemoryContexts;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		private readonly IDynamoDbRepository<UserDTO> _inMemoryUserContext;

		private readonly IServiceProvider _scopeProvider;

		public UserUseCaseTests()
		{
			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_inMemoryUserContext = new InMemoryUserContext();
			services.AddScoped(_ => _inMemoryUserContext);

			var _rootProvider = services.BuildServiceProvider(validateScopes: true);
			_scopeProvider = _rootProvider.CreateScope().ServiceProvider;
		}

		[SetUp]
		public void Setup()
		{
			((InMemoryUserContext)_inMemoryUserContext).UseDefaultValues();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			_inMemoryUserContext.Dispose();
		}
	}
}