using Bug.Chatter.Domain.Errors;
using System.Text.RegularExpressions;

namespace Bug.Chatter.Domain.ValueObjects
{
	public sealed record NumericCode
	{
		public string Value { get; }

		private NumericCode(string value)
		{
			Value = value;
		}

		public static NumericCode Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new DomainException(string.Format(ErrorReason.Code.NumericCodeRequired, nameof(NumericCode)));

			if (!IsValid(value))
				throw new DomainException(string.Format(ErrorReason.Code.NumericCodeInvalid, nameof(NumericCode)));

			return new NumericCode(value);
		}

		public static NumericCode Generate() => new(new Random().Next(999_999).ToString("000000"));

		private static bool IsValid(string value)
		{
			var regex = new Regex(@"^\d{6}$");

			return regex.IsMatch(value);
		}

		public override string ToString() => Value;
	}
}
