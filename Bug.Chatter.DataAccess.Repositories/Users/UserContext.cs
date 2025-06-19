using Amazon.DynamoDBv2;
using Bug.Chatter.DataAccess.Repositories.DynamoDb;
using Bug.Chatter.Domain.SeedWork.StringBuilders;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.DataAccess.Repositories.Users
{
	public class UserContext : GenericDynamoDbRepository<UserDTO>
	{
		public UserContext(IAmazonDynamoDB ddbClient)
			: base(ddbClient,
				  Database.ChatterTableName)
		{
			
		}
	}
}
