using Bug.Chatter.Domain.SeedWork;
using Newtonsoft.Json;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.EventStores.Mappers
{
	public static class EventStoreDbMapperExtensions
	{
		public static IDomainEvent ToDomainEvent(this EventStoreEntry entry)
		{
			var obj = JsonConvert.DeserializeObject(entry.EventData, entry.EventType);

			if (obj is null)
			{
				throw new ArgumentException(
					$"{nameof(entry)}: Erro ao deserializar." +
					$" EventData: {entry.EventData} - EventType: {entry.EventType}");
			}

			return (IDomainEvent)obj;
		}

		public static EventStoreEntry ToEventStoreEntry<TAggregate>(this IDomainEvent domainEvent)
		{
			return EventStoreEntry.Create<TAggregate>(
				aggregatePk: domainEvent.AggregatePk,
				aggregateSk: domainEvent.AggregateSk,
				version: domainEvent.Version,
				eventType: domainEvent.EventType,
				timestamp: domainEvent.Timestamp,
				eventData: domainEvent.EventData);
		}
	}
}
