using Bug.Chatter.Application;
using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users;
using Bug.Chatter.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	public class UserUseCaseTests
	{
		private readonly IServiceCollection _services;
		private readonly IServiceProvider _serviceProvider;

		public UserUseCaseTests()
		{
			_services = new ServiceCollection()
				.AddUserApplicationServices();
		}

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public async Task User_ShouldBeCreated_UseCaseTest()
		{
			// Arrange
			var command = new CreateUserCommand("Victor Bugueno", "+55 (11) 97562-3736");
			
			var mockRepository = new Mock<IUserRepository>();
			mockRepository
				.Setup(repo => repo.SafePutAsync(It.IsAny<User>()))
				.Returns(Task.FromResult(true));

			var useCase = new CreateUserUseCase(
				mockRepository.Object,
				_serviceProvider.GetRequiredService<ICommandMapper<CreateUserCommand, User>>());

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			Assert.That(result.HasFailed, Is.False);
		}
	}
}