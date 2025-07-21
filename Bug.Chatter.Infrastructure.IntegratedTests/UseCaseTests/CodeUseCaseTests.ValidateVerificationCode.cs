using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users.ValidateVerificationCode;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class CodeUseCaseTests
	{
		[Test]
		public async Task ValidateVerificationCode_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var validateVerificationCodeUseCase = _scopeProvider.GetRequiredService<ValidateVerificationCodeUseCase>();
			var command = new ValidateVerificationCodeCommand(Guid.Parse("094b1c2d-ee50-4c68-a18a-8dca65d450c6"), "123456");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserWithCodesSpecification();

			// Act
			var result = await validateVerificationCodeUseCase.HandleAsync(command);
			var userCodes = (await userRepository.GetByUserIdAsync(command.UserId, spec))?.Codes;

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

				Assert.That(userCodes, Is.Not.Null);
				Assert.That(userCodes!.Any(userCode => userCode.VerificationCode == command.VerificationCode));
			});
		}

		[Test]
		public async Task ValidateVerificationCode_WithWrongVerificationCode_ShouldReturnRejectedResult()
		{
			// Arrange
			var validateVerificationCodeUseCase = _scopeProvider.GetRequiredService<ValidateVerificationCodeUseCase>();
			var command = new ValidateVerificationCodeCommand(Guid.Parse("094b1c2d-ee50-4c68-a18a-8dca65d450c6"), "999999");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserWithCodesSpecification();

			// Act
			var result = await validateVerificationCodeUseCase.HandleAsync(command);
			var userCodes = (await userRepository.GetByUserIdAsync(command.UserId, spec))?.Codes;

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(userCodes, Is.Not.Null);
				Assert.That(!userCodes!.Any(userCode => userCode.VerificationCode == command.VerificationCode));
			});
		}

		[Test]
		public async Task ValidateVerificationCode_WithInvalidUser_ShouldReturnRejectedResult()
		{
			// Arrange
			var validateVerificationCodeUseCase = _scopeProvider.GetRequiredService<ValidateVerificationCodeUseCase>();
			var command = new ValidateVerificationCodeCommand(Guid.NewGuid(), "123456");
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserWithCodesSpecification();

			// Act
			var result = await validateVerificationCodeUseCase.HandleAsync(command);
			var userSaved = await userRepository.GetByUserIdAsync(command.UserId, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(userSaved, Is.Null);
			});
		}

		[Test]
		public void ValidateVerificationCode_WithInvalidVerificationCode_ShouldThrowsInvalidVerificationCode()
		{
			// Arrange & Act & Assert
			Assert.Throws<ArgumentException>(() => new ValidateVerificationCodeCommand(Guid.Parse("094b1c2d-ee50-4c68-a18a-8dca65d450c6"), "abcdef"));
		}
	}
}