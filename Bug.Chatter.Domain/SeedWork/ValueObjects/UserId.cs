using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.SeedWork.ValueObjects
{
	public sealed record UserId
	{
		public Guid Value { get; }

		private UserId(Guid value)
		{
			Value = value;
		}

		public static UserId Create(Guid value)
		{
			if (value == Guid.Empty)
				throw new DomainException(string.Format(ErrorReason.User.IdRequired, nameof(UserId)));

			return new UserId(value);
		}

		public static UserId Generate() => new UserId(Guid.NewGuid());

		public override string ToString() => Value.ToString();

		public static implicit operator Guid(UserId userId) => userId.Value;

		public static explicit operator UserId(Guid value) => Create(value);
	}
}
