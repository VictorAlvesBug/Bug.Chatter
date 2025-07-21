using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.Users.ValueObjects
{
	public sealed record UserStatus
	{
		public static readonly UserStatus Draft = new(nameof(Draft));
		public static readonly UserStatus Registered = new(nameof(Registered));
		public static readonly UserStatus Deleted = new(nameof(Deleted));

		public string Value { get; }

		private UserStatus(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(string.Format(ErrorReason.UserCode.StatusRequired, nameof(UserStatus)));

			Value = value;
		}

		public static UserStatus From(string value)
		{
			return value switch
			{
				nameof(Draft) => Draft,
				nameof(Registered) => Registered,
				nameof(Deleted) => Deleted,
				_ => throw new ArgumentException(string.Format(ErrorReason.UserCode.StatusInvalid, nameof(UserStatus), value))
			};
		}

		public override string ToString() => Value;
	}
}
