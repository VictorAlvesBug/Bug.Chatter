using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.Common.ValueObjects
{
	public sealed record Content
	{
		private const int _maxContentLength = 500;
		public string Value { get; private set; }

		private Content(string value)
		{
			Value = value;
		}

		public static Content Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(string.Format(ErrorReason.Message.ContentRequired, nameof(Content)));

			if (value.Length > _maxContentLength)
				throw new ArgumentException(string.Format(ErrorReason.Message.ContentTooLarge, nameof(Content), _maxContentLength));

			return new Content(value);
		}

		public override string ToString() => Value;
	}
}
