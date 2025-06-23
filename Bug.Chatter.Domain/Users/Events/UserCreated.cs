using Bug.Chatter.Domain.SeedWork;
using Bug.Chatter.Domain.Users.ValueObjects;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Bug.Chatter.Domain.Users.Events
{
	public sealed record UserCreated(
		UserId Id,
		Name Name,
		PhoneNumber PhoneNumber,
		int Version
		) : IDomainEvent
	{
		public string AggregatePk => UserPk.Create(Id).Value;

		public string AggregateSk => "user-mainSchema-v0";// TODO: Ajustar para centralizar essa UserSk

		public Type EventType => typeof(UserCreated);

		public DateTimeOffset Timestamp => DateTimeOffset.UtcNow;

		public string EventData => JsonConvert.SerializeObject(
			new
			{
				Id,
				Name,
				PhoneNumber
			}
		);
	}
}
