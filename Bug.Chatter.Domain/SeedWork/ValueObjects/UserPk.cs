using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.SeedWork.ValueObjects
{
	public sealed record UserPk
	{
		public const string Prefix = "user";
		public UserId Value { get; }

		private UserPk(UserId value)
		{
			Value = value;
		}

		public static UserPk Create(UserId value)
		{
			if(value is null)
				throw new DomainException(string.Format(ErrorReason.User.IdRequired, nameof(UserId)));

			return new UserPk(value);
		}

		public override string ToString() => $"{Prefix}-{Value}";
	}
}
