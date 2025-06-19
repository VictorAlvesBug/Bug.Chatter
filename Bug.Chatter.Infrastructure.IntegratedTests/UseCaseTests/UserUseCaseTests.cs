using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	public class UserUseCaseTests
	{
		private readonly IServiceCollection _services;
		private readonly IServiceProvider _serviceProvider;

		public UserUseCaseTests()
		{
			_services = new ServiceCollection();
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

			var useCase = _serviceProvider.GetRequiredService<CreateUserUseCase>();

			// Act
			var result = await useCase.HandleAsync(command);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
			Assert.That(result.HasFailed, Is.False);
		}
	}
}