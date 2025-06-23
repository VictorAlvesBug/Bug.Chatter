using Bug.Chatter.Domain.SeedWork;

namespace Bug.Chatter.Domain.EventStores
{
	public interface IEventStoreRepository<TAggregate>
	{
		public Task AppendAsync(IEnumerable<IDomainEvent> events);
		public Task<IEnumerable<IDomainEvent>> GetEventsAsync(string pk);
	}
}
