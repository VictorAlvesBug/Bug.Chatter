using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users;
using Bug.Chatter.Application.Users.CreateUser;
using Bug.Chatter.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Application.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddCreateUserServices();
			return services;
		}

		public static IServiceCollection AddCreateUserServices(this IServiceCollection services)
		{
			services.AddScoped<ICommandMapper<CreateUserCommand, User>,
				CreateUserCommandMapper>();
			services.AddScoped<IUseCase<CreateUserCommand, Result<UserModel>>,
				CreateUserUseCase>();
			services.AddScoped<CreateUserUseCase>();
			return services;
		}
	}
}
