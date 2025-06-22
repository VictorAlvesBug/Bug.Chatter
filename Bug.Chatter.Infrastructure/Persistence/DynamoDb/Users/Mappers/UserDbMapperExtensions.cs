using Bug.Chatter.Domain.SeedWork.ValueObjects;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users.Mappers
{
	public static class UserDbMapperExtensions
	{
		public static User ToDomain(this UserDTO dto)
		{
			return User.CreateFromPrimitives(
				id: UserId.Create(Guid.Parse(dto.Id)),
				name: Name.Create(dto.Name),
				phoneNumber: PhoneNumber.Create(dto.PhoneNumber),
				version: dto.Version
			);
		}

		public static UserDTO ToDTO(this User user, string userSk)
		{
			return new UserDTO(
				pk: user.Pk.Value,
				sk: userSk,
				id: user.Id.Value,
				name: user.Name.Value,
				phoneNumber: user.PhoneNumber.Value,
				version: user.Version
			);
		}

	}
}
