using Bug.Chatter.Application.Aggregates.Codes.SendNewCode;
using Bug.Chatter.Application.Aggregates.Codes.ValidateCode;
using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.IntegratedTests.SeedWork;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class CodeUseCaseTests
	{
		private readonly DatabaseMock _databaseMock;

		private readonly Mock<IDynamoDbRepository<CodeDTO>> _mockCodeContext;

		private readonly IServiceProvider _scopeProvider;

		public CodeUseCaseTests()
		{
			_databaseMock = new();

			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_mockCodeContext = new Mock<IDynamoDbRepository<CodeDTO>>();
			services.OverrideWithMockContext<IDynamoDbRepository<CodeDTO>, CodeDTO>(_databaseMock, _mockCodeContext);

			var _rootProvider = services.BuildServiceProvider(validateScopes: true);
			_scopeProvider = _rootProvider.CreateScope().ServiceProvider;
		}

		[SetUp]
		public void Setup()
		{
			_databaseMock.UseDefaultCodes();
			Mock.Get(_mockCodeContext.Object).Invocations.Clear();
		}


		[Test]
		public async Task MultiCaseTest()
		{
			// Arrange & Act
			var sendNewCodeUseCase = _scopeProvider.GetRequiredService<SendNewCodeUseCase>();
			var sendNewCodeCommand = new SendNewCodeCommand("+55 (11) 97562-3736");
			var sendNewCodeResult = await sendNewCodeUseCase.HandleAsync(sendNewCodeCommand);

			var lastSafePutInvocation = _mockCodeContext.GetLastInvocationOf(nameof(_mockCodeContext.Object.SafePutAsync));
			Assert.That(lastSafePutInvocation, Is.Not.Null);

			var receivedCodeDTO = lastSafePutInvocation!.Arguments[0] as CodeDTO;
			Assert.That(receivedCodeDTO, Is.Not.Null);

			var validateCodeUseCase = _scopeProvider.GetRequiredService<ValidateCodeUseCase>();
			var validateCodeCommand = new ValidateCodeCommand(
				receivedCodeDTO!.PhoneNumber,
				receivedCodeDTO!.NumericCode
			);

			var validateCodeResult = await validateCodeUseCase.HandleAsync(validateCodeCommand);

			// Assert
			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.AtLeastOnce);
		}
	}
}