using Bug.Chatter.Application.Aggregates.Codes.ValidateCode;
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
		public async Task ValidateCode_WithValidCommand_ShouldReturnSuccessResult()
		{
			// Arrange
			var validateCodeUseCase = _scopeProvider.GetRequiredService<ValidateCodeUseCase>();
			var command = new ValidateCodeCommand("+55 (11) 97562-3736", "123456");

			// Act
			var result = await validateCodeUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			});

			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.AtLeastOnce);
		}

		[Test]
		public async Task ValidateCode_WithWrongNumericCode_ShouldReturnRejectedResult()
		{
			// Arrange
			var validateCodeUseCase = _scopeProvider.GetRequiredService<ValidateCodeUseCase>();
			var command = new ValidateCodeCommand("+55 (11) 97562-3736", "999999");

			// Act
			var result = await validateCodeUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));
			});

			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Once);
		}

		[Test]
		public async Task ValidateCode_WithInvalidPhoneNumber_ShouldReturnFailureResult()
		{
			// Arrange
			var validateCodeUseCase = _scopeProvider.GetRequiredService<ValidateCodeUseCase>();
			var command = new ValidateCodeCommand("12345678", "123456");

			// Act
			var result = await validateCodeUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
			});

			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Never);
		}

		[Test]
		public async Task ValidateCode_WithInvalidNumericCode_ShouldReturnFailureResult()
		{
			// Arrange
			var validateCodeUseCase = _scopeProvider.GetRequiredService<ValidateCodeUseCase>();
			var command = new ValidateCodeCommand("+55 (11) 97562-3736", "abcdef");

			// Act
			var result = await validateCodeUseCase.HandleAsync(command);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
			});

			_mockCodeContext.Verify(
				r => r.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()),
				Times.Never);
		}
	}
}