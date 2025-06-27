namespace Bug.Chatter.Application.Aggregates.Codes.ValidateCode
{
	public class ValidateCodeCommand
	{
		public ValidateCodeCommand(
			string phoneNumber,
			string numericCode)
		{
			PhoneNumber = phoneNumber;
			NumericCode = numericCode;
		}

		public string PhoneNumber { get; }
		public string NumericCode { get; }
	}
}
