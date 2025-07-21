namespace Bug.Chatter.Application.Users
{
	public class UserModel
	{
		public UserModel(
			string id,
			string name,
			string phoneNumber,
			string status,
			int version)
		{
			Id = id;
			Name = name;
			PhoneNumber = phoneNumber;
			Status = status;
			Version = version;
		}

		public string Id { get; }
		public string Name { get; }
		public string PhoneNumber { get; }
		public string Status { get; }
		public int Version { get; }
	}
}
