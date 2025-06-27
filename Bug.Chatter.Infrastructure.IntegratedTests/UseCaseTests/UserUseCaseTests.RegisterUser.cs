using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users.RegisterUser;
using Bug.Chatter.Application.Users.ValidateNew;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		[Test]
		public async Task RegisterUser_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var registerUserUseCase = _scopeProvider.GetRequiredService<RegisterUserUseCase>();
			var command = new RegisterUserCommand("Maria Alice", "+55 (11) 6966-8083");

			// Act
			var result = await registerUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});

			_mockUserContext.Verify(
				r => r.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Once);
			_mockUserContext.Verify(r => r.SafePutAsync(It.IsAny<UserDTO>()), Times.Once);
		}

		[Test]
		public async Task RegisterUser_WithDuplicatedPhoneNumber_ShouldReturnRejectedResult()
		{
			// Arrange
			var registerUserUseCase = _scopeProvider.GetRequiredService<RegisterUserUseCase>();
			var command = new RegisterUserCommand("Victor Bugueno2", "+55 (11) 97562-3736");

			// Act
			var result = await registerUserUseCase.HandleAsync(command);

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
	}
}