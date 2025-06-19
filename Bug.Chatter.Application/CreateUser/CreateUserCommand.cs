using Bug.Chatter.Application.SeedWork.UseCaseStructure;

namespace Bug.Chatter.Application.Users
{
	public class CreateUserCommand : IInput
	{
		public CreateUserCommand(
			string name,
			string phoneNumber)
		{
			Name = name;
			PhoneNumber	= phoneNumber;
		}

		public string Name { get; }
		public string PhoneNumber { get; }
	}
}
