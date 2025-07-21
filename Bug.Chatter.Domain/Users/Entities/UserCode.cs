using Bug.Chatter.Domain.Common;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Users.Entities
{
	public class UserCode // não herda de Entity<TId>
	{
		private const int _minutesToExpire = 10;

		public VerificationCode VerificationCode { get; protected init; }
		public CodeStatus Status { get; protected init; }
		public int Version { get; private set; }
		public DateTime CreatedAt { get; protected init; }
		public DateTime ExpiresAt { get; protected init; }

		public UserCode()
			: this(
				  verificationCode: VerificationCode.Generate(),
				  status: CodeStatus.NotSentYet,
				  version: 1,
				  createdAt: DateTime.UtcNow,
				  expiresAt: DateTime.UtcNow.AddMinutes(_minutesToExpire))
		{ }

		public UserCode(
			VerificationCode verificationCode,
			CodeStatus status,
			int version,
			DateTime createdAt,
			DateTime expiresAt)
		{
			VerificationCode = verificationCode;
			Status = status;
			Version = version;
			CreatedAt = createdAt;
			ExpiresAt = expiresAt;
		}

		public bool IsValid() => ExpiresAt >= DateTime.UtcNow;

		public void IncrementVersion() => Version++;
	}
}
