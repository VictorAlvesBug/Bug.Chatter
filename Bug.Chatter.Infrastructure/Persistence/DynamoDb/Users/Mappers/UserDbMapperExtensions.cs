using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Users.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;
using System.Globalization;
using System.Runtime.Serialization;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users.Mappers
{
	public static class UserDbMapperExtensions
	{
		public static User ToDomain(this UserDTO dto)
		{
			var createdAtDateTime = DateTime.ParseExact(
				dto.CreatedAt,
				DatabaseSettings.DateTimeFormat,
				CultureInfo.InvariantCulture);

			//var

			return User.Rehydrate(
				id: UserId.Create(Guid.Parse(dto.Id)),
				name: Name.Create(dto.Name),
				phoneNumber: PhoneNumber.Create(dto.PhoneNumber),
				version: dto.Version,
				createdAt: createdAtDateTime
			);
		}

		public static UserDTO ToDTO(this User domain, string userSk)
		{
			return new UserDTO(
				pk: domain.Pk.Value,
				sk: userSk,
				id: domain.Id.Value,
				name: domain.Name.Value,
				phoneNumber: domain.PhoneNumber.Value,
				version: domain.Version,
				createdAt: domain.CreatedAt.ToUtcStringDateTime()
			);
		}

	}
}
