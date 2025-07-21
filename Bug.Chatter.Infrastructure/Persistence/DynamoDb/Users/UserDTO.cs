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
			string status,
			int version,
			string createdAt)
			: base(pk, sk, version)
		{
			Id = id;
			Name = name;
			PhoneNumber = phoneNumber;
			Status = status;
			CreatedAt = createdAt;
		}

		public string Id { get; }
		public string Name { get; }
		public string PhoneNumber { get; }
		public string Status { get; }
		public string CreatedAt { get; }
	}
}
