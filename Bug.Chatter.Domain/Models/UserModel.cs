namespace Bug.Chatter.Domain.Models
{
	public class UserModel
	{
		public UserModel(
			string id,
			string name,
			string phoneNumber)
		{
			Id = id;
			Name = name;
			PhoneNumber = phoneNumber;
		}

		public string Id { get; }
		public string Name { get; }
		public string PhoneNumber { get; }
	}
}
