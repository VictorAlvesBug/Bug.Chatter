namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public class UserCodeDTO : EntityDTO
	{
		public UserCodeDTO(
			string pk,
			string sk,
			string verificationCode,
			string status,
			int version,
			string createdAt,
			string expiresAt,
			long ttl)
			: base(pk, sk, version)
		{
			VerificationCode = verificationCode;
			Status = status;
			CreatedAt = createdAt;
			ExpiresAt = expiresAt;
			TTL = ttl;
		}

		public string VerificationCode { get; }
		public string Status { get; }
		public string CreatedAt { get; }
		public string ExpiresAt { get; }
		public long TTL { get; }
	}
}
