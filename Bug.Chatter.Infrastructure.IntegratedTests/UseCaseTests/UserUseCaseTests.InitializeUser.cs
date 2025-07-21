using Bug.Chatter.Application.Users.InitializeUser;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.SeedWork.Specifications;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using System.Xml.Linq;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.Entities;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		[Test]
		public async Task InitializeUser_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var initializeUserUseCase = _scopeProvider.GetRequiredService<InitializeUserUseCase>();
			var command = new InitializeUserCommand("Maria Alice", "+55 (11) 6966-8083");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var result = await initializeUserUseCase.HandleAsync(command);
			var savedUser = await userRepository.GetByPhoneNumberAsync(command.PhoneNumber, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

				Assert.That(savedUser, Is.Not.Null);
				Assert.That(savedUser!.Name, Is.EqualTo(command.Name));
				Assert.That(savedUser!.PhoneNumber, Is.EqualTo(command.PhoneNumber));
			});

		}

		[Test]
		public async Task InitializeUser_WithSamePhoneNumberOfDraftOrDeletedUser_ShouldReturnSuccessResult()
		{
			// Arrange
			var initializeUserUseCase = _scopeProvider.GetRequiredService<InitializeUserUseCase>();
			var command = new InitializeUserCommand("Fátima Alves Bugueno", "+55 (11) 98237-5687");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var result = await initializeUserUseCase.HandleAsync(command);
			var savedUser = await userRepository.GetByPhoneNumberAsync(command.PhoneNumber, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

				Assert.That(savedUser, Is.Not.Null);
				Assert.That(savedUser!.Name, Is.EqualTo(command.Name));
				Assert.That(savedUser!.PhoneNumber, Is.EqualTo(command.PhoneNumber));
			});
		}

		[Test]
		public async Task InitializeUser_WithSamePhoneNumberOfRegisteredUser_ShouldReturnRejectedResult()
		{
			// Arrange
			var initializeUserUseCase = _scopeProvider.GetRequiredService<InitializeUserUseCase>();
			var command = new InitializeUserCommand("Victor Alves Bugueno", "+55 (11) 97562-3736");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var result = await initializeUserUseCase.HandleAsync(command);
			var savedUser = await userRepository.GetByPhoneNumberAsync(command.PhoneNumber, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(savedUser, Is.Not.Null);
				Assert.That(User.StatusAllowInitializeUser(savedUser!.Status), Is.False);
			});
		}

		[Test]
		public void InitializeUser_WithInvalidPhoneNumber_ShouldThrowsInvalidPhoneNumber()
		{
			// Arrange & Act & Assert
			Assert.Throws<ArgumentException>(() => new InitializeUserCommand("Fulano de Tal", "12345678"));
		}
	}
}