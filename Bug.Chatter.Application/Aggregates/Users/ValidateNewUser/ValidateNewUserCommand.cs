namespace Bug.Chatter.Application.Aggregates.Users.ValidateNewUser
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
