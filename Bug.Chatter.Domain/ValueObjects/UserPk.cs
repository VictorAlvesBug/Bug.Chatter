using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.ValueObjects
{
	public sealed record UserPk
	{
		public const string Prefix = "user";
		private BaseId _userId { get; }
		public string Value => $"{Prefix}-{_userId}";

		private UserPk(BaseId value)
		{
			_userId = value;
		}

		public static UserPk Create(BaseId userId)
		{
			if (userId is null)
				throw new DomainException(string.Format(ErrorReason.BaseId.IdRequired, nameof(BaseId)));

			return new UserPk(userId);
		}

		public override string ToString() => Value;
	}
}
