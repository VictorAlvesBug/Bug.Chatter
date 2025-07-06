using Amazon.DynamoDBv2;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Microsoft.Extensions.Caching.Memory;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	internal class UserContext(
		IAmazonDynamoDB ddbClient,
		IMemoryCache memoryCache) : GenericDynamoDbRepository<UserDTO>(ddbClient, DatabaseSettings.ChatterTableName, memoryCache), IUserContext
	{
	}
}
