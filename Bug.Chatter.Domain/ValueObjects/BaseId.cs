using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.ValueObjects
{
	public sealed record BaseId
	{
		private Guid _guid { get; }

		public string Value => _guid.ToString();

		private BaseId(Guid guid)
		{
			_guid = guid;
		}

		public static BaseId Create(Guid guid)
		{
			if (guid == Guid.Empty)
				throw new DomainException(string.Format(ErrorReason.BaseId.IdRequired, nameof(BaseId)));

			return new BaseId(guid);
		}

		public static BaseId Generate() => new(Guid.NewGuid());

		public override string ToString() => Value;

		public static implicit operator Guid(BaseId baseId) => baseId._guid;

		public static explicit operator BaseId(Guid guid) => Create(guid);
	}
}
