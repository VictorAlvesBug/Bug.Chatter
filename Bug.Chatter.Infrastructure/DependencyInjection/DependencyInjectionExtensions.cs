using Amazon.DynamoDBv2;
using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.Aggregates.Users;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes;
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

			services.AddScoped<ICodeContext, CodeContext>();
			services.AddScoped<ICodeRepository, CodeRepository>();

			services.AddScoped<IUserContext, UserContext>();
			services.AddScoped<IUserRepository, UserRepository>();

			//services.AddScoped<IChatContext, ChatContext>();
			//services.AddScoped<IChatRepository, ChatRepository>();

			//services.AddScoped<IMessageContext, MessageContext>();
			//services.AddScoped<IMessageRepository, MessageRepository>();


			return services;
		}
	}
}
