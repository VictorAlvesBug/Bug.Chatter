using Bug.Chatter.Domain.Aggregates.Users;

namespace Bug.Chatter.Application.Aggregates.Users
{
	public static class UserApiMapperExtensions
	{
		public static UserModel ToModel(this User user)
		{
			return new UserModel(
				id: user.Id.Value,
				name: user.Name.Value,
				phoneNumber: user.PhoneNumber.Value,
				version: user.Version
			);
		}
	}
}
