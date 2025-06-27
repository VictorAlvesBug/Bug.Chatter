using Bug.Chatter.Application.SeedWork.UseCaseStructure;

namespace Bug.Chatter.Application.Users.RegisterUser
{
	public class RegisterUserCommand
	{
		public RegisterUserCommand(
			string name,
			string phoneNumber)
		{
			Name = name;
			PhoneNumber = phoneNumber;
		}

		public string Name { get; }
		public string PhoneNumber { get; }
	}
}
