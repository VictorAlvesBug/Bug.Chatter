using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.Users.ValueObjects
{
	public sealed record Name
	{
		public string Value { get; }

		private Name(string value)
		{
			Value = value;
		}

		public static Name Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new DomainException(string.Format(ErrorReason.User.NameRequired, nameof(Name)));

			return new Name(value);
		}

		public override string ToString() => Value;
	}
}
