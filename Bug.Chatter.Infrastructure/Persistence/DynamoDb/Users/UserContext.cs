using Amazon.DynamoDBv2;
using Bug.Chatter.Domain.SeedWork.StringBuilders;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public class UserContext : GenericDynamoDbRepository<UserDTO>, IUserContext
	{
		public UserContext(IAmazonDynamoDB ddbClient)
			: base(ddbClient,
				  Database.ChatterTableName)
		{
			
		}
	}
}
