using Bug.Chatter.Domain.Errors;
using System.Text.RegularExpressions;

namespace Bug.Chatter.Domain.Users.ValueObjects
{
	public sealed record VerificationCode
	{
		public string Value { get; }

		private VerificationCode(string value)
		{
			Value = value;
		}

		public static VerificationCode Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(string.Format(ErrorReason.UserCode.VerificationCodeRequired, nameof(VerificationCode)));

			if (!IsValid(value))
				throw new ArgumentException(string.Format(ErrorReason.UserCode.VerificationCodeInvalid, nameof(VerificationCode)));

			return new VerificationCode(value);
		}

		public static VerificationCode Generate() => new(new Random().Next(999_999).ToString("000000"));

		private static bool IsValid(string value)
		{
			var regex = new Regex(@"^\d{6}$");

			return regex.IsMatch(value);
		}

		public override string ToString() => Value;
	}
}
