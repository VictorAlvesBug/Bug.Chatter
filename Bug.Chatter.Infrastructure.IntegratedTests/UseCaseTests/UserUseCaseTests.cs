using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users.CreateUser;
using Bug.Chatter.Domain.EventStores;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Users.ValueObjects;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public class UserUseCaseTests
	{
		private Mock<IUserRepository> _mockUserRepository;
		private Mock<IEventStoreRepository<User>> _mockUserEventStoreRepository;
		private Mock<ICommandMapper<CreateUserCommand, User>> _mockUserMapper;
		private CreateUserUseCase _createUserUseCase;

		[SetUp]
		public void Setup()
		{
			_mockUserRepository = new Mock<IUserRepository>();
			_mockUserEventStoreRepository = new Mock<IEventStoreRepository<User>>();
			_mockUserMapper = new Mock<ICommandMapper<CreateUserCommand, User>>();

			_createUserUseCase = new CreateUserUseCase(
				_mockUserRepository.Object,
				_mockUserEventStoreRepository.Object,
				_mockUserMapper.Object);
		}

		[Test]
		public async Task HandleAsync_WithValidCommand_ShouldReturnSuccesseResult()
		{
			// Arrange
			var command = new CreateUserCommand("Victor Bugueno", "+55 (11) 97562-3736");
			var expectedUser = User.CreateNew(
				name: Name.Create(command.Name), 
				phoneNumber: PhoneNumber.Create(command.PhoneNumber)
			);

			_mockUserMapper
				.Setup(m => m.Map(It.IsAny<CreateUserCommand>()))
				.Returns(expectedUser);

			_mockUserRepository
				.Setup(r => r.SafePutAsync(It.IsAny<User>()))
				.Returns(Task.FromResult(true));

			// Act
			var result = await _createUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});

			_mockUserRepository.Verify(r => r.SafePutAsync(It.IsAny<User>()), Times.Once);
		}

		[Test]
		public async Task HandleAsync_WhenRepositoryFails_ShouldReturnFailureResult()
		{
			// Arrange
			var command = new CreateUserCommand("Victor Bugueno", "+55 (11) 97562-3736");
			var expectedUser = User.CreateNew(
				name: Name.Create(command.Name),
				phoneNumber: PhoneNumber.Create(command.PhoneNumber)
			);

			_mockUserMapper.Setup(m => m.Map(It.IsAny<CreateUserCommand>()))
					  .Returns(expectedUser);

			_mockUserRepository
				.Setup(r => r.SafePutAsync(It.IsAny<User>()))
				.Throws<Exception>();

			// Act
			var result = await _createUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
				Assert.That(result.Reasons, Is.Not.Empty);
			});
		}
	}
}