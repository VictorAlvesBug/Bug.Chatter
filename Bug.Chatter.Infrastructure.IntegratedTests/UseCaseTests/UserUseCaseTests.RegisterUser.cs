using Bug.Chatter.Application.Users.RegisterUser;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users.ValueObjects;

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
			var command = new RegisterUserCommand(Guid.Parse("ea9983c8-be00-4307-93ad-635d961de718"));
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var savedUserBefore = await userRepository.GetByUserIdAsync(command.UserId, spec);
			var result = await registerUserUseCase.HandleAsync(command);
			var savedUserAfter = await userRepository.GetByUserIdAsync(command.UserId, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

				Assert.That(savedUserBefore, Is.Not.Null);
				Assert.That(savedUserBefore!.Status, Is.EqualTo(UserStatus.Draft));

				Assert.That(savedUserAfter, Is.Not.Null);
				Assert.That(savedUserAfter!.Status, Is.EqualTo(UserStatus.Registered));
			});
		}

		[Test]
		public async Task RegisterUser_WithNonDraftUser_ShouldReturnRejectedResult()
		{
			// Arrange
			var registerUserUseCase = _scopeProvider.GetRequiredService<RegisterUserUseCase>();
			var command = new RegisterUserCommand(Guid.Parse("094b1c2d-ee50-4c68-a18a-8dca65d450c6"));
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var savedUserBefore = await userRepository.GetByUserIdAsync(command.UserId, spec);
			var result = await registerUserUseCase.HandleAsync(command);
			var savedUserAfter = await userRepository.GetByUserIdAsync(command.UserId, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(savedUserBefore, Is.Not.Null);
				Assert.That(savedUserBefore!.Status, Is.Not.EqualTo(UserStatus.Draft));

				Assert.That(savedUserAfter, Is.Not.Null);
				Assert.That(savedUserAfter!.Status, Is.Not.EqualTo(UserStatus.Draft));
				Assert.That(savedUserAfter!.Status, Is.EqualTo(savedUserBefore!.Status));
			});
		}

		[Test]
		public async Task RegisterUser_WithInvalidUser_ShouldReturnRejectedResult()
		{
			// Arrange
			var registerUserUseCase = _scopeProvider.GetRequiredService<RegisterUserUseCase>();
			var command = new RegisterUserCommand(Guid.NewGuid());
			var userRepository = _scopeProvider.GetRequiredService<IUserRepository>();
			var spec = new UserOnlySpecification();

			// Act
			var result = await registerUserUseCase.HandleAsync(command);
			var savedUser = await userRepository.GetByUserIdAsync(command.UserId, spec);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Status, Is.EqualTo(ResultStatus.Rejected));

				Assert.That(savedUser, Is.Null);
			});
		}
	}
}