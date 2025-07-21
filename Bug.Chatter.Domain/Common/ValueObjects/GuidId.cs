using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.Common.ValueObjects
{
	public sealed record GuidId
	{
		private Guid _guid { get; }

		public string Value => _guid.ToString();

		private GuidId(Guid guid)
		{
			_guid = guid;
		}

		public static GuidId Create(Guid guid)
		{
			if (guid == Guid.Empty)
				throw new ArgumentNullException(string.Format(ErrorReason.GuidId.IdRequired, nameof(GuidId)));

			return new GuidId(guid);
		}

		public static GuidId Generate() => new(Guid.NewGuid());

		public override string ToString() => Value;

		public static implicit operator Guid(GuidId baseId) => baseId._guid;

		public static explicit operator GuidId(Guid guid) => Create(guid);
	}
}
