using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.CreateUser;
using Bug.Chatter.Application.Users;
using Bug.Chatter.DataAccess.Repositories;
using Bug.Chatter.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Application
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddUserApplicationServices(this IServiceCollection services)
		{
			return services
				.AddScoped<ICommandMapper<CreateUserCommand, User>, CreateUserCommandMapper>()
				.AddUserDataAccessServices();
		}
	}
}
