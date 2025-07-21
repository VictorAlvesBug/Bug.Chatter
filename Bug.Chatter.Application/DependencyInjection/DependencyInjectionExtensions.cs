using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users;
using Bug.Chatter.Application.Users.InitializeUser;
using Bug.Chatter.Application.Users.LoginUser;
using Bug.Chatter.Application.Users.RegisterUser;
using Bug.Chatter.Application.Users.SendVerificationCode;
using Bug.Chatter.Application.Users.ValidateVerificationCode;
using Bug.Chatter.Domain.Users.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Application.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddSendVerificationCodeServices();
			services.AddValidateVerificationCodeServices();
			services.AddLoginUserServices();
			services.AddRegisterUserServices();
			services.AddInitializeUserServices();
			return services;
		}

		public static IServiceCollection AddSendVerificationCodeServices(this IServiceCollection services)
		{
			services.AddScoped<IUseCase<SendVerificationCodeCommand, Result<UserModel>>,
				SendVerificationCodeUseCase>();
			services.AddScoped<SendVerificationCodeUseCase>();
			return services;
		}

		public static IServiceCollection AddValidateVerificationCodeServices(this IServiceCollection services)
		{
			services.AddScoped<IUseCase<ValidateVerificationCodeCommand, Result<UserModel>>,
				ValidateVerificationCodeUseCase>();
			services.AddScoped<ValidateVerificationCodeUseCase>();
			return services;
		}

		public static IServiceCollection AddLoginUserServices(this IServiceCollection services)
		{
			services.AddScoped<IUseCase<LoginUserCommand, Result<UserModel>>,
				LoginUserUseCase>();
			services.AddScoped<LoginUserUseCase>();
			return services;
		}

		public static IServiceCollection AddRegisterUserServices(this IServiceCollection services)
		{
			services.AddScoped<IUseCase<RegisterUserCommand, Result<UserModel>>,
				RegisterUserUseCase>();
			services.AddScoped<RegisterUserUseCase>();
			return services;
		}

		public static IServiceCollection AddInitializeUserServices(this IServiceCollection services)
		{
			services.AddScoped<ICommandMapper<InitializeUserCommand, User>,
				InitializeUserCommandMapper>();
			services.AddScoped<IUseCase<InitializeUserCommand, Result<UserModel>>,
				InitializeUserUseCase>();
			services.AddScoped<InitializeUserUseCase>();
			return services;
		}
	}
}
