using Bug.Chatter.Application.Users.LoginUser;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;

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
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var result = await loginUserUseCase.HandleAsync(command);
			var savedUser = await userRepository.GetByPhoneNumberAsync(command.PhoneNumber, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

				Assert.That(savedUser, Is.Not.Null);
				Assert.That(savedUser!.Status, Is.EqualTo(UserStatus.Registered));
			});
		}

		[Test]
		public async Task LoginUser_WithUnknownPhoneNumber_ShouldReturnRejectedResult()
		{
			// Arrange
			var loginUserUseCase = _scopeProvider.GetRequiredService<LoginUserUseCase>();
			var command = new LoginUserCommand("+55 (11) 6966-8083");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var result = await loginUserUseCase.HandleAsync(command);
			var savedUser = await userRepository.GetByPhoneNumberAsync(command.PhoneNumber, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(savedUser, Is.Null);
			});
		}

		[Test]
		public async Task LoginUser_WithInvalidPhoneNumber_ShouldReturnFailureResult()
		{
			// Arrange & Act & Assert
			Assert.Throws<ArgumentException>(() => new LoginUserCommand("12345678"));
		}
	}
}