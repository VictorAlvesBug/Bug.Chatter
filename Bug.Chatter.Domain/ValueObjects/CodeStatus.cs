﻿using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.ValueObjects
{
	public sealed record CodeStatus
	{
		public static readonly CodeStatus NotSentYet = new("NotSentYet");
		public static readonly CodeStatus Sent = new("Sent");

		public string Value { get; }

		private CodeStatus(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new DomainException(string.Format(ErrorReason.Code.StatusRequired, nameof(CodeStatus)));

			Value = value;
		}

		public static CodeStatus From(string value)
		{
			return value switch
			{
				"NotSentYet" => NotSentYet,
				"Sent" => Sent,
				_ => throw new DomainException(string.Format(ErrorReason.Code.StatusInvalid, nameof(CodeStatus)))
			};
		}

		public override string ToString() => Value;
	}
}
