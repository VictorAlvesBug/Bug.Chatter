using Amazon.DynamoDBv2;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Microsoft.Extensions.Caching.Memory;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	internal class UserContext : GenericDynamoDbRepository<UserDTO>, IUserContext
	{
		public UserContext(
			IAmazonDynamoDB ddbClient,
			IMemoryCache memoryCache)
			: base(ddbClient, DatabaseSettings.ChatterTableName, memoryCache)
		{
			
		}
	}
}
