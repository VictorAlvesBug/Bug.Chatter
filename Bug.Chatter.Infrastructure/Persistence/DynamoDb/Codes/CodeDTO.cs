namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes
{
	public class CodeDTO : EntityDTO
	{
		public CodeDTO(
			string pk,
			string sk,
			string numericCode,
			string phoneNumber,
			string status,
			int version,
			string createdAt,
			string expiresAt,
			long ttl)
			: base(pk, sk)
		{
			NumericCode = numericCode;
			PhoneNumber = phoneNumber;
			Status = status;
			Version = version;
			CreatedAt = createdAt;
			ExpiresAt = expiresAt;
			TTL = ttl;
		}

		public string NumericCode { get; }
		public string PhoneNumber { get; }
		public string Status { get; }
		public int Version { get; }
		public string CreatedAt { get; }
		public string ExpiresAt { get; }
		public long TTL { get; }
	}
}
