namespace Bug.Chatter.Application.Aggregates.Users.RegisterUser
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
