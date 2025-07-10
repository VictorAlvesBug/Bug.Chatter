namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public class UserDTO : EntityDTO
	{
		public UserDTO(
			string pk,
			string sk,
			string id,
			string name,
			string phoneNumber,
			int version,
			string createdAt)
			: base(pk, sk)
		{
			Id = id;
			Name = name;
			PhoneNumber = phoneNumber;
			Version = version;
			CreatedAt = createdAt;
		}

		public string Id { get; }
		public string Name { get; }
		public string PhoneNumber { get; }
		public int Version { get; }
		public string CreatedAt { get; }
	}
}
