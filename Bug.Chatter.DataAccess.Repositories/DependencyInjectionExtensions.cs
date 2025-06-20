using Bug.Chatter.DataAccess.Repositories.Users;
using Bug.Chatter.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.DataAccess.Repositories
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddUserDataAccessServices(this IServiceCollection services)
		{
			return services
				.AddScoped<UserContext>()
				.AddScoped<IUserRepository, UserRepository>();
		}
	}
}
