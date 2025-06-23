using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.EventStores
{
	public interface IEventStoreContext : IDynamoDbRepository<EventStoreEntry>
	{
	}
}
