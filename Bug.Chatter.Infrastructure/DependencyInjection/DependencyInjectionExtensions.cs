using Bug.Chatter.Domain.Users;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bug.Chatter.Infrastructure.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			return services
				.AddScoped<IDynamoDbTable, DynamoDbTable>()
				.AddScoped<IUserContext, UserContext>()
				.AddScoped<IUserRepository, UserRepository>()
				.RegisterDynamoRepositories();
		}

		private static IServiceCollection RegisterDynamoRepositories(this IServiceCollection services)
		{
			var entityTypes = new[] { typeof(UserDTO), /*typeof(ChatDTO), typeof(MessageDTO)*/ };

			foreach (var type in entityTypes)
			{
				var repositoryType = typeof(GenericDynamoDbRepository<>).MakeGenericType(type);
				var interfaceType = typeof(IDynamoDbRepository<>).MakeGenericType(type);

				services.AddScoped(interfaceType, repositoryType);
			}

			return services;
		}
	}
}
