using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.EventStores.ValueObjects;
using Bug.Chatter.Domain.SeedWork;
using Newtonsoft.Json;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.EventStores;

public sealed class EventStoreEntry
{
	public readonly EventStorePk Pk;
	public readonly EventStoreSk Sk;
	public readonly string AggregatePk;
	public readonly string AggregateSk;
	public readonly int Version;
	public readonly Type EventType;
	public readonly DateTimeOffset Timestamp;
	public readonly string EventData;

	private EventStoreEntry(
		string aggregatePk,
		string aggregateSk,
		int version,
		Type eventType,
		DateTimeOffset timestamp,
		string eventData)
	{
		AggregatePk = aggregatePk;
		AggregateSk = aggregateSk;
		Version = version;
		Pk = EventStorePk.Create(AggregatePk, AggregateSk);
		Sk = EventStoreSk.Create(Version);
		EventType = eventType;
		Timestamp = timestamp;
		EventData = eventData;
	}

	public static EventStoreEntry Create<TAggregate>(
		string aggregatePk,
		string aggregateSk,
		int version,
		Type eventType,
		DateTimeOffset timestamp,
		string eventData)
	{
		CreationValidations<TAggregate>(aggregatePk, aggregateSk, version, eventType, timestamp, eventData);

		return new EventStoreEntry(aggregatePk, aggregateSk, version, eventType, timestamp, eventData);
	}

	public static EventStoreEntry Rehydrate(
		string aggregatePk,
		string aggregateSk,
		int version,
		Type eventType,
		DateTimeOffset timestamp,
		string eventData)
	{
		return new EventStoreEntry(aggregatePk, aggregateSk, version, eventType, timestamp, eventData);
	}

	private static void CreationValidations<TAggregate>(
		string aggregatePk,
		string aggregateSk,
		int version,
		Type eventType,
		DateTimeOffset timestamp,
		string eventData)
	{
		if (string.IsNullOrWhiteSpace(aggregatePk))
			throw new DomainException(string.Format(ErrorReason.EventStore.AggregatePkRequired, nameof(aggregatePk)));

		if (string.IsNullOrWhiteSpace(aggregateSk))
			throw new DomainException(string.Format(ErrorReason.EventStore.AggregateSkRequired, nameof(aggregateSk)));

		if (version <= 0)
			throw new DomainException(string.Format(ErrorReason.EventStore.VersionInvalid, nameof(version)));

		// TODO: Validar EventType conforme comandos que podem ser aplicados ao agregado
		if (eventType is not IDomainEvent)
			throw new DomainException(string.Format(ErrorReason.EventStore.EventTypeInvalid, nameof(eventType)));

		if (timestamp == DateTimeOffset.MinValue)
			throw new DomainException(string.Format(ErrorReason.EventStore.TimestampRequired, nameof(timestamp)));

		if (eventData is not null)
		{
			try
			{
				var settings = new JsonSerializerSettings
				{
					MissingMemberHandling = MissingMemberHandling.Error,
					NullValueHandling = NullValueHandling.Include,
					Error = (_, args) => args.ErrorContext.Handled = false
				};

				JsonConvert.DeserializeObject<TAggregate>(eventData, settings);
			}
			catch (JsonSerializationException)
			{
				throw new DomainException(string.Format(ErrorReason.EventStore.EventDataInvalid, nameof(eventData)));
			}
			catch (JsonReaderException)
			{
				throw new DomainException(string.Format(ErrorReason.EventStore.EventDataInvalid, nameof(eventData)));
			}
		}
	}
}
