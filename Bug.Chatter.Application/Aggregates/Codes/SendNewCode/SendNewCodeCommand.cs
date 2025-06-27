namespace Bug.Chatter.Application.Aggregates.Codes.SendNewCode
{
	public class SendNewCodeCommand
	{
		public SendNewCodeCommand(
			string phoneNumber)
		{
			PhoneNumber = phoneNumber;
		}

		public string PhoneNumber { get; }
	}
}
