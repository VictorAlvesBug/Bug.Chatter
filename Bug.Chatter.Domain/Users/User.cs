using Bug.Chatter.Domain.SeedWork.ValueObjects;

namespace Bug.Chatter.Domain.Users
{
	public class User
	{
		public UserPk Pk { get; }
		public UserSk Sk { get; }
		public UserId Id { get; }
		public Name Name { get; private set; }
		public PhoneNumber PhoneNumber { get; private set; }
		public int Version { get; }

		private User(
			Name name,
			PhoneNumber phoneNumber)
			: this(
				id: UserId.Generate(),
				name,
				phoneNumber,
				version: 1) { }

		private User(
			UserId id,
			Name name,
			PhoneNumber phoneNumber,
			int version)
		{
			Id = id;
			Pk = UserPk.Create(Id);
			Sk = UserSk.Create();
			Name = name;
			PhoneNumber = phoneNumber;
			Version = version;
		}

		public static User CreateNew(Name name, PhoneNumber phoneNumber)
		{
			return new User(name, phoneNumber);
		}

		public static User CreateFromPrimitives(
			UserId id,
			Name name,
			PhoneNumber phoneNumber,
			int version)
		{
			return new User(id, name, phoneNumber, version);
		}
	}
}
