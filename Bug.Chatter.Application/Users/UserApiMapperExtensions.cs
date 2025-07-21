using Bug.Chatter.Domain.Users.Entities;

namespace Bug.Chatter.Application.Users
{
	public static class UserApiMapperExtensions
	{
		public static UserModel ToModel(this User user)
		{
			return new UserModel(
				id: user.Id.Value,
				name: user.Name.Value,
				phoneNumber: user.PhoneNumber.Value,
				status: user.Status.ToString(),
				version: user.Version
			);
		}
	}
}
