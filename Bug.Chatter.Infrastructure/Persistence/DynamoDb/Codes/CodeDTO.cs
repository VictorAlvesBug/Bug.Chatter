namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes
{
	public class CodeDTO
	{
		public CodeDTO(
			string pk,
			string sk,
			string numericCode,
			string phoneNumber,
			string status,
			int version,
			string createdAt,
			string expireAt)
		{
			PK = pk;
			SK = sk;
			NumericCode = numericCode;
			PhoneNumber = phoneNumber;
			Status = status;
			Version = version;
			CreatedAt = createdAt;
			ExpireAt = expireAt;
		}

		public string PK { get; }
		public string SK { get; }
		public string NumericCode { get; }
		public string PhoneNumber { get; }
		public string Status { get; }
		public int Version { get; }
		public string CreatedAt { get; }
		public string ExpireAt { get; }
	}
}
