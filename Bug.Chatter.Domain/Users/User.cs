using Bug.Chatter.Domain.Users.ValueObjects;
using System;

namespace Bug.Chatter.Domain.Users
{
	public class User
	{
		public UserPk Pk { get; protected init; }
		public UserId Id { get; protected init; }
		public Name Name { get; private set; }
		public PhoneNumber PhoneNumber { get; private set; }
		public int Version { get; private set; }
		public DateTime CreatedAt { get; protected init; }

		private User(
			Name name,
			PhoneNumber phoneNumber)
			: this(
				id: UserId.Generate(),
				name,
				phoneNumber,
				version: 1,
				createdAt: DateTime.UtcNow)
		{ }

		private User(
			UserId id,
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
			UserId id,
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
