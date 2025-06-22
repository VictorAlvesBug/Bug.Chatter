using Amazon.DynamoDBv2;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	internal class UserContext : GenericDynamoDbRepository<UserDTO>, IUserContext
	{
		public UserContext(
			IAmazonDynamoDB ddbClient)
			: base(ddbClient, DatabaseSettings.ChatterTableName)
		{
			
		}
	}
}
