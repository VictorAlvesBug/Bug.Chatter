using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Aggregates.Codes
{
	public class Code
	{
		private const int _minutesToExpire = 10;

		public CodePk Pk { get; protected init; }
		public NumericCode NumericCode { get; protected init; }
		public PhoneNumber PhoneNumber { get; protected init; }
		public CodeStatus Status { get; protected init; }
		public int Version { get; protected init; }
		public DateTime CreatedAt { get; protected init; }
		public DateTime ExpiresAt { get; protected init; }

		private Code(PhoneNumber phoneNumber)
			: this(
				  numericCode: NumericCode.Generate(),
				  phoneNumber,
				  status: CodeStatus.NotSentYet,
				  version: 1,
				  createdAt: DateTime.UtcNow,
				  expiresAt: DateTime.UtcNow.AddMinutes(_minutesToExpire))
		{ }

		private Code(
			NumericCode numericCode,
			PhoneNumber phoneNumber,
			CodeStatus status,
			int version,
			DateTime createdAt,
			DateTime expiresAt)
		{
			Pk = CodePk.Create(numericCode);
			NumericCode = numericCode;
			PhoneNumber = phoneNumber;
			Status = status;
			Version = version;
			CreatedAt = createdAt;
			ExpiresAt = expiresAt;
		}

		public static Code Rehydrate(
			NumericCode numericCode,
			PhoneNumber phoneNumber,
			CodeStatus status,
			int version,
			DateTime createdAt,
			DateTime expiresAt)
		{
			return new Code(numericCode, phoneNumber, status, version, createdAt, expiresAt);
		}

		public static Code CreateNew(PhoneNumber phoneNumber)
		{
			return new Code(phoneNumber);
		}

		public bool IsExpired() => ExpiresAt < DateTime.UtcNow;

		public bool PhoneNumbersMatch(string value)
		{
			PhoneNumber commandPhoneNumber = PhoneNumber.Create(value);
			return commandPhoneNumber.Value == this.PhoneNumber.Value;
		}
	}
}
