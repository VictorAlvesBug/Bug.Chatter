using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users.SendVerificationCode;
using Bug.Chatter.Application.Users.ValidateVerificationCode;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.IntegratedTests.SeedWork.InMemoryContexts;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class CodeUseCaseTests
	{
		private readonly IDynamoDbRepository<UserCodeDTO> _inMemoryUserCodeContext;
		private readonly IDynamoDbRepository<UserDTO> _inMemoryUserContext;

		private readonly IServiceProvider _scopeProvider;

		public CodeUseCaseTests()
		{
			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_inMemoryUserCodeContext = new InMemoryUserCodeContext();
			services.AddScoped(_ => _inMemoryUserCodeContext);

			_inMemoryUserContext = new InMemoryUserContext();
			services.AddScoped(_ => _inMemoryUserContext);

			var _rootProvider = services.BuildServiceProvider(validateScopes: true);
			_scopeProvider = _rootProvider.CreateScope().ServiceProvider;
		}

		[SetUp]
		public void Setup()
		{
			((InMemoryUserCodeContext)_inMemoryUserCodeContext).UseDefaultValues();
			((InMemoryUserContext)_inMemoryUserContext).UseDefaultValues();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			_inMemoryUserCodeContext.Dispose();
			_inMemoryUserContext.Dispose();
		}


		[Test]
		public async Task MultiCaseTest()
		{
			var userId = Guid.Parse("094b1c2d-ee50-4c68-a18a-8dca65d450c6");
			var sendVerificationCodeUseCase = _scopeProvider.GetRequiredService<SendVerificationCodeUseCase>();
			var sendVerificationCodeCommand = new SendVerificationCodeCommand(userId);
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserWithCodesSpecification();

			var userCodesBefore = (await userRepository.GetByUserIdAsync(sendVerificationCodeCommand.UserId, spec))?.Codes;
			var sendVerificationCodeResult = await sendVerificationCodeUseCase.HandleAsync(sendVerificationCodeCommand);
			var userCodesAfter = (await userRepository.GetByUserIdAsync(sendVerificationCodeCommand.UserId, spec))?.Codes;

			Assert.Multiple(() =>
			{
				Assert.That(userCodesBefore, Is.Not.Null);
				Assert.That(userCodesAfter, Is.Not.Null);
			});
			
			Assert.That(userCodesAfter, Has.Count.EqualTo(userCodesBefore.Count + 1));

			var validateVerificationCodeUseCase = _scopeProvider.GetRequiredService<ValidateVerificationCodeUseCase>();
			var validateVerificationCodeCommand = new ValidateVerificationCodeCommand(
				userId,
				userCodesAfter!.LastOrDefault()?.VerificationCode.Value
			);

			var validateVerificationCodeResult = await validateVerificationCodeUseCase.HandleAsync(validateVerificationCodeCommand);

			Assert.That(validateVerificationCodeResult.Status, Is.EqualTo(ResultStatus.Success));
		}
	}
}