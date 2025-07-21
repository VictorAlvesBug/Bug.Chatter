using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users.SendVerificationCode;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class CodeUseCaseTests
	{
		[Test]
		public async Task SendVerificationCode_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var sendVerificationCodeUseCase = _scopeProvider.GetRequiredService<SendVerificationCodeUseCase>();
			var command = new SendVerificationCodeCommand(Guid.Parse("094b1c2d-ee50-4c68-a18a-8dca65d450c6"));
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserWithCodesSpecification();

			// Act
			var userCodesBefore = (await userRepository.GetByUserIdAsync(command.UserId, spec))?.Codes;
			var result = await sendVerificationCodeUseCase.HandleAsync(command);
			var userCodesAfter = (await userRepository.GetByUserIdAsync(command.UserId, spec))?.Codes;


			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

				Assert.That(userCodesBefore, Is.Not.Null);
				Assert.That(userCodesAfter, Is.Not.Null);
				Assert.That(userCodesAfter!, Has.Count.EqualTo(userCodesBefore!.Count + 1));
			});
		}

		[Test]
		public async Task SendVerificationCode_WithInvalidUser_ShouldReturnRejectedResult()
		{
			// Arrange
			var sendVerificationCodeUseCase = _scopeProvider.GetRequiredService<SendVerificationCodeUseCase>();
			var command = new SendVerificationCodeCommand(Guid.NewGuid());
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserWithCodesSpecification();

			// Act
			var result = await sendVerificationCodeUseCase.HandleAsync(command);
			var userSaved = await userRepository.GetByUserIdAsync(command.UserId, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(userSaved, Is.Null);
			});
		}
	}
}