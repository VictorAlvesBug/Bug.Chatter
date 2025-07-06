using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Aggregates.Users
{
	public class User
	{
		public UserPk Pk { get; protected init; }
		public BaseId Id { get; protected init; }
		public Name Name { get; private set; }
		public PhoneNumber PhoneNumber { get; private set; }
		public int Version { get; private set; }
		public DateTime CreatedAt { get; protected init; }

		private User(
			Name name,
			PhoneNumber phoneNumber)
			: this(
				id: BaseId.Generate(),
				name,
				phoneNumber,
				version: 1,
				createdAt: DateTime.UtcNow)
		{ }

		private User(
			BaseId id,
			Name name,
			PhoneNumber phoneNumber,
			int version,
			DateTime createdAt)
		{
			Id = id;
			Pk = UserPk.Create(Id);
			Name = name;
			PhoneNumber = phoneNumber;
			Version = version;
			CreatedAt = createdAt;
		}

		public static User Rehydrate(
			BaseId id,
			Name name,
			PhoneNumber phoneNumber,
			int version,
			DateTime createdAt)
		{
			return new User(id, name, phoneNumber, version, createdAt);
		}

		public static User CreateNew(Name name, PhoneNumber phoneNumber)
		{
			return new User(name, phoneNumber);
		}
	}
}
