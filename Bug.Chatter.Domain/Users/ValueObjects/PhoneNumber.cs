using Bug.Chatter.Domain.Errors;
using System.Text.RegularExpressions;

namespace Bug.Chatter.Domain.Users.ValueObjects
{
	public sealed record PhoneNumber
	{
		public string Value { get; }

		private PhoneNumber(string value)
		{
			Value = value;
		}

		public static PhoneNumber Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(string.Format(ErrorReason.User.PhoneNumberRequired, nameof(PhoneNumber)));


			if (!IsValid(value))
				throw new ArgumentException(string.Format(ErrorReason.User.PhoneNumberInvalid, nameof(PhoneNumber)));

			return new PhoneNumber(value);
		}

		private static bool IsValid(string value)
		{
			var regex = new Regex(@"^\+(\d{1,3})\s\(\d{2}\)\s\d{4,5}-\d{4}$");

			return regex.IsMatch(value);
		}

		public override string ToString() => Value;
	}
}
