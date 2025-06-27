using Bug.Chatter.Application.Aggregates.Codes;
using Bug.Chatter.Application.Aggregates.Codes.SendNewCode;
using Bug.Chatter.Application.Aggregates.Codes.ValidateCode;
using Bug.Chatter.Application.Aggregates.Users;
using Bug.Chatter.Application.Aggregates.Users.LoginUser;
using Bug.Chatter.Application.Aggregates.Users.RegisterUser;
using Bug.Chatter.Application.Aggregates.Users.ValidateNewUser;
using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Aggregates.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Application.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddSendNewCodeServices();
			services.AddValidateCodeServices();
			services.AddLoginUserServices();
			services.AddRegisterUserServices();
			services.AddValidateNewUserServices();
			return services;
		}

		public static IServiceCollection AddSendNewCodeServices(this IServiceCollection services)
		{
			services.AddScoped<IUseCase<SendNewCodeCommand, Result<CodeModel>>,
				SendNewCodeUseCase>();
			services.AddScoped<SendNewCodeUseCase>();
			return services;
		}

		public static IServiceCollection AddValidateCodeServices(this IServiceCollection services)
		{
			services.AddScoped<IUseCase<ValidateCodeCommand, Result<CodeModel>>,
				ValidateCodeUseCase>();
			services.AddScoped<ValidateCodeUseCase>();
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
