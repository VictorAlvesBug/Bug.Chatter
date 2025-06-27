namespace Bug.Chatter.Application.Aggregates.Users.LoginUser
{
	public class LoginUserCommand
	{
		public LoginUserCommand(
			string phoneNumber)
		{
			PhoneNumber = phoneNumber;
		}

		public string PhoneNumber { get; }
	}
}
