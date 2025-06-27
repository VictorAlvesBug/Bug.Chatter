using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.ValueObjects
{
	public sealed record Name
	{
		private const int _maxNameLength = 50;
		public string Value { get; private set; }

		private Name(string value)
		{
			Value = value;
		}

		public static Name Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new DomainException(string.Format(ErrorReason.User.NameRequired, nameof(Name)));

			if (value.Length > _maxNameLength)
				throw new DomainException(string.Format(ErrorReason.User.NameTooLarge, nameof(Name), _maxNameLength));

			return new Name(value);
		}

		public override string ToString() => Value;
	}
}
