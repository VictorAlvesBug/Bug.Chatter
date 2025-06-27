using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users;
using Bug.Chatter.Application.Users.RegisterUser;
using Bug.Chatter.Application.Users.ValidateNew;
using Bug.Chatter.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Application.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddRegisterUserServices();
			services.AddValidateNewUserServices();
			return services;
		}

		public static IServiceCollection AddRegisterUserServices(this IServiceCollection services)
		{
			services.AddScoped<ICommandMapper<RegisterUserCommand, User>,
				RegisterUserCommandMapper>();
			services.AddScoped<IUseCase<RegisterUserCommand, Result<UserModel>>,
				RegisterUserUseCase>();
			services.AddScoped<RegisterUserUseCase>();
			return services;
		}

		public static IServiceCollection AddValidateNewUserServices(this IServiceCollection services)
		{
			services.AddScoped<ICommandMapper<ValidateNewUserCommand, User>,
				ValidateNewUserCommandMapper>();
			services.AddScoped<IUseCase<ValidateNewUserCommand, Result<UserModel>>,
				ValidateNewUserUseCase>();
			services.AddScoped<ValidateNewUserUseCase>();
			return services;
		}
	}
}
