namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public class UserDTO
	{
		public UserDTO(
			string pk,
			string sk,
			string id,
			string name,
			string phoneNumber,
			int version)
		{
			PK = pk;
			SK = sk;
			Id = id;
			Name = name;
			PhoneNumber = phoneNumber;
			Version = version;
		}

		public string PK { get; }
		public string SK { get; }
		public string Id { get; }
		public string Name { get; }
		public string PhoneNumber { get; }
		public int Version { get; }
	}
}
