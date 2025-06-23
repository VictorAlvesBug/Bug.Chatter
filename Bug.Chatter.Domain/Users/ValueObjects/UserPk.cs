using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.Users.ValueObjects
{
	public sealed record UserPk
	{
		public const string Prefix = "user";
		private UserId _userId { get; }
		public string Value => $"{Prefix}-{_userId}";

		private UserPk(UserId value)
		{
			_userId = value;
		}

		public static UserPk Create(UserId value)
		{
			if (value is null)
				throw new DomainException(string.Format(ErrorReason.User.IdRequired, nameof(UserId)));

			return new UserPk(value);
		}

		public override string ToString() => Value;
	}
}
