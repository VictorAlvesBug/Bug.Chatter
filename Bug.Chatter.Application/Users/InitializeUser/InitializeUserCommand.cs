using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Application.Users.InitializeUser
{
	public class InitializeUserCommand
	{
		public InitializeUserCommand(
			string name,
			string phoneNumber)
		{
			Name = Name.Create(name);
			PhoneNumber = PhoneNumber.Create(phoneNumber);
		}

		public Name Name { get; }
		public PhoneNumber PhoneNumber { get; }
	}
}
