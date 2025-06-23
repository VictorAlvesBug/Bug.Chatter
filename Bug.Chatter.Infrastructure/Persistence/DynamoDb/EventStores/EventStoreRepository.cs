using Bug.Chatter.Domain.EventStores;
using Bug.Chatter.Domain.SeedWork;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.EventStores.Mappers;
using Newtonsoft.Json;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.EventStores
{
	public class EventStoreRepository<TAggregate> : IEventStoreRepository<TAggregate>
	{
		private readonly IEventStoreContext _eventStoreContext;

		public EventStoreRepository(IEventStoreContext eventStoreContext)
		{
			_eventStoreContext = eventStoreContext;
		}

		public async Task AppendAsync(IEnumerable<IDomainEvent> events)
		{
			foreach (var entry in events.Select(e => e.ToEventStoreEntry<TAggregate>()))
			{
				await _eventStoreContext.SafePutAsync(entry);
			}
		}

		public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(string pk)
		{
			var entries = await _eventStoreContext.ListByPartitionKeyAsync(pk);

			return entries.Select(entry => entry.ToDomainEvent());
		}
	}
}
