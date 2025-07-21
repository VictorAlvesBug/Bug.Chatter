namespace Bug.Chatter.Application.Users
{
	public class UserCodeModel
	{
		public UserCodeModel(
			string verificationCode,
			string phoneNumber,
			int version)
		{
			VerificationCode = verificationCode;
			PhoneNumber = phoneNumber;
			Version = version;
		}

		public string VerificationCode { get; }
		public string PhoneNumber { get; }
		public int Version { get; }
	}
}
