using Bug.Chatter.Application.SeedWork.UseCaseStructure;

namespace Bug.Chatter.Application.Users.ValidateNew
{
	public class ValidateNewUserCommand
	{
		public ValidateNewUserCommand(
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
