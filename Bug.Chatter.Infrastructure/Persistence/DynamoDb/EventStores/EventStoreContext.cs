using Amazon.DynamoDBv2;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.EventStores
{
	public class EventStoreContext : GenericDynamoDbRepository<EventStoreEntry>, IEventStoreContext
	{
		public EventStoreContext(
			IAmazonDynamoDB ddbClient)
			: base(ddbClient, DatabaseSettings.ChatterTableName)
		{

		}
	}
}
