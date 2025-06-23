using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.EventStores.ValueObjects
{
	public sealed record EventStoreSk
	{
		public const string Prefix = "version";
		private int _version { get; }
		public string Value => $"{Prefix}-{_version}";

		private EventStoreSk(int version)
		{
			_version = version;
		}

		public static EventStoreSk Create(int version)
		{
			if (version <= 0)
				throw new DomainException(string.Format(ErrorReason.EventStore.VersionInvalid, nameof(version)));

			return new EventStoreSk(version);
		}

		public override string ToString() => Value;
	}
}
