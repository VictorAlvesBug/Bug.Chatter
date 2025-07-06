using Bug.Chatter.Application.Aggregates.Codes.SendNewCode;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class CodeUseCaseTests
	{
		[Test]
		public async Task SendNewCode_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var sendNewCodeUseCase = _scopeProvider.GetRequiredService<SendNewCodeUseCase>();
			var command = new SendNewCodeCommand("+55 (11) 97562-3736");

			// Act
			var result = await sendNewCodeUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});

			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.AtLeastOnce);

			_mockCodeContext.Verify(
				r => r.SafePutAsync(It.IsAny<CodeDTO>()),
				Times.Once);
		}

		[Test]
		public async Task SendNewCode_WithInvalidPhoneNumber_ShouldReturnFailureResult()
		{
			// Arrange
			var sendNewCodeUseCase = _scopeProvider.GetRequiredService<SendNewCodeUseCase>();
			var command = new SendNewCodeCommand("12345678");

			// Act
			var result = await sendNewCodeUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
			});

			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Never);

			_mockCodeContext.Verify(
				r => r.SafePutAsync(It.IsAny<CodeDTO>()),
				Times.Never);
		}
	}
}