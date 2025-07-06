using Bug.Chatter.Application.Aggregates.Users.ValidateNewUser;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		[Test]
		public async Task ValidateNewUser_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var validateNewUserUseCase = _scopeProvider.GetRequiredService<ValidateNewUserUseCase>();
			var command = new ValidateNewUserCommand("Maria Alice", "+55 (11) 6966-8083");

			// Act
			var result = await validateNewUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});

			_mockUserContext.Verify(
				r => r.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Once);
			_mockUserContext.Verify(r => r.SafePutAsync(It.IsAny<UserDTO>()), Times.Never);
		}

		[Test]
		public async Task ValidateNewUser_WithDuplicatedPhoneNumber_ShouldReturnRejectedResult()
		{
			// Arrange
			var validateNewUserUseCase = _scopeProvider.GetRequiredService<ValidateNewUserUseCase>();
			var command = new ValidateNewUserCommand("Victor Bugueno2", "+55 (11) 97562-3736");

			// Act
			var result = await validateNewUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));
			});

			_mockUserContext.Verify(
				r => r.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Once);
			_mockUserContext.Verify(r => r.SafePutAsync(It.IsAny<UserDTO>()), Times.Never);
		}

		[Test]
		public async Task ValidateNewUser_WithInvalidPhoneNumber_ShouldReturnFailureResult()
		{
			// Arrange
			var validateNewUserUseCase = _scopeProvider.GetRequiredService<ValidateNewUserUseCase>();
			var command = new ValidateNewUserCommand("Victor Bugueno3", "12345678");

			// Act
			var result = await validateNewUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
			});

			_mockUserContext.Verify(
				r => r.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Never);
			_mockUserContext.Verify(r => r.SafePutAsync(It.IsAny<UserDTO>()), Times.Never);
		}
	}
}