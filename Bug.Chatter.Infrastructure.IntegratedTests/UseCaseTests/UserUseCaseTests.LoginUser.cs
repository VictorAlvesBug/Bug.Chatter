using Bug.Chatter.Application.Aggregates.Users.LoginUser;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class UserUseCaseTests
	{
		[Test]
		public async Task LoginUser_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var loginUserUseCase = _scopeProvider.GetRequiredService<LoginUserUseCase>();
			var command = new LoginUserCommand("+55 (11) 97562-3736");

			// Act
			var result = await loginUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});

			_mockUserContext.Verify(
				r => r.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Once);
		}

		[Test]
		public async Task LoginUser_WithUnknownPhoneNumber_ShouldReturnRejectedResult()
		{
			// Arrange
			var loginUserUseCase = _scopeProvider.GetRequiredService<LoginUserUseCase>();
			var command = new LoginUserCommand("+55 (11) 6966-8083");

			// Act
			var result = await loginUserUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));
			});

			_mockUserContext.Verify(
				r => r.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Once);
		}
	}
}