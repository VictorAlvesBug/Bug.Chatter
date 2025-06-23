using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.EventStores.ValueObjects
{
	public sealed record EventStorePk
	{
		public const string Prefix = "event";
		private string _aggregatePk { get; }
		private string _aggregateSk { get; }
		public string Value => $"{Prefix}-{_aggregatePk}-{_aggregateSk}";

		private EventStorePk(string aggregatePk, string aggregateSk)
		{
			_aggregatePk = aggregatePk;
			_aggregateSk = aggregateSk;
		}

		public static EventStorePk Create(string aggregatePk, string aggregateSk)
		{
			if (aggregatePk is null)
				throw new DomainException(string.Format(ErrorReason.EventStore.AggregatePkRequired, nameof(aggregatePk)));

			if (aggregateSk is null)
				throw new DomainException(string.Format(ErrorReason.EventStore.AggregateSkRequired, nameof(aggregateSk)));

			return new EventStorePk(aggregatePk, aggregateSk);
		}

		public override string ToString() => Value;
	}
}
