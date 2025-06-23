namespace Bug.Chatter.Domain.SeedWork
{
	public class AggregateRoot<TPk, TSk>
	{
		public TPk Pk { get; set; }
		public TSk Sk { get; set; }
		public int Version { get; set; }
		public List<IDomainEvent> DomainEvents { get; set; } = [];
	}
}
