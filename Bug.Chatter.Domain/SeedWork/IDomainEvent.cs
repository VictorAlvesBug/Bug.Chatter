namespace Bug.Chatter.Domain.SeedWork
{
	public interface IDomainEvent
	{
		string AggregatePk { get; }
		string AggregateSk { get; }
		int Version { get; }
		Type EventType { get; }
		DateTimeOffset Timestamp { get; }
		string EventData { get; }
	}
}
