namespace Bug.Chatter.Application.Aggregates.Codes
{
	public class CodeModel
	{
		public CodeModel(
			string numericCode,
			string phoneNumber,
			int version)
		{
			NumericCode = numericCode;
			PhoneNumber = phoneNumber;
			Version = version;
		}

		public string NumericCode { get; }
		public string PhoneNumber { get; }
		public int Version { get; }
	}
}
