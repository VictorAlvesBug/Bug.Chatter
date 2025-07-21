using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Application.Users.LoginUser
{
	public class LoginUserCommand
	{
		public LoginUserCommand(
			string phoneNumber)
		{
			PhoneNumber = PhoneNumber.Create(phoneNumber);
		}

		public PhoneNumber PhoneNumber { get; }
	}
}
