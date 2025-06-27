using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.ValueObjects
{
	public sealed record CodePk
	{
		public const string Prefix = "code";
		private NumericCode _numericCode { get; }
		public string Value => $"{Prefix}-{_numericCode}";

		private CodePk(NumericCode numericCode)
		{
			_numericCode = numericCode;
		}

		public static CodePk Create(NumericCode numericCode)
		{
			if (numericCode is null)
				throw new DomainException(string.Format(ErrorReason.Code.NumericCodeRequired, nameof(NumericCode)));

			return new CodePk(numericCode);
		}

		public override string ToString() => Value;
	}
}
