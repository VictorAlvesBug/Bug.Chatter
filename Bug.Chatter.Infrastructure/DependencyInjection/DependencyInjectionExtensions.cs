using Amazon.DynamoDBv2;
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
			services.AddScoped<IDynamoDbTable, DynamoDbTable>();
			services.AddAWSService<IAmazonDynamoDB>();
			services.AddMemoryCache();

			services.AddScoped<IDynamoDbRepository<UserDTO>, GenericDynamoDbRepository<UserDTO>>();
			services.AddScoped<IDynamoDbRepository<UserCodeDTO>, GenericDynamoDbRepository<UserCodeDTO>>();
			services.AddScoped<IUserRepository, UserRepository>();

			//services.AddScoped<IDynamoDbRepository<ChatDTO>, ChatContext>();
			//services.AddScoped<IChatRepository, ChatRepository>();


			return services;
		}
	}
}
