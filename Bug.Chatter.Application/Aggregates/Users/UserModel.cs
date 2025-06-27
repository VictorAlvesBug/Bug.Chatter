namespace Bug.Chatter.Application.Aggregates.Users
{
	public class UserModel
	{
		public UserModel(
			string id,
			string name,
			string phoneNumber,
			int version)
		{
			Id = id;
			Name = name;
			PhoneNumber = phoneNumber;
			Version = version;
		}

		public string Id { get; }
		public string Name { get; }
		public string PhoneNumber { get; }
		public int Version { get; }
	}
}
