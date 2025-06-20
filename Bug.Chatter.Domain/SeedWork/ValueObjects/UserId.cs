using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.SeedWork.ValueObjects
{
	public sealed record UserId
	{
		private Guid _guid { get; }
		
		public string Value => _guid.ToString();
		
		private UserId(Guid guid)
		{
			_guid = guid;
		}

		public static UserId Create(Guid guid)
		{
			if (guid == Guid.Empty)
				throw new DomainException(string.Format(ErrorReason.User.IdRequired, nameof(UserId)));

			return new UserId(guid);
		}

		public static UserId Generate() => new UserId(Guid.NewGuid());

		public override string ToString() => Value;

		public static implicit operator Guid(UserId userId) => userId._guid;

		public static explicit operator UserId(Guid guid) => Create(guid);
	}
}
