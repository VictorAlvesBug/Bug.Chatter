using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.Users.Entities;
using Bug.Chatter.Domain.Users.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public static class UserDbMapperExtensions
	{
		public static User ToDomain(this UserDTO dto, IEnumerable<UserCodeDTO> codeDtos)
		{
			var codes = codeDtos?.Any() == true
				? codeDtos.Select(codeDto => codeDto.ToDomain()).ToList()
				: [];

			return User.Rehydrate(
				id: GuidId.Create(Guid.Parse(dto.Id)),
				name: Name.Create(dto.Name),
				phoneNumber: PhoneNumber.Create(dto.PhoneNumber),
				status: UserStatus.From(dto.Status),
				version: dto.Version,
				createdAt: dto.CreatedAt.ToBrazilianDateTime(),
				codes: codes
			);
		}

		public static UserDTO ToDTO(this User domain)
		{
			return new UserDTO(
				pk: KeyFactory.UserPk(domain.Id),
				sk: KeyFactory.UserSk(),
				id: domain.Id.Value,
				name: domain.Name.Value,
				phoneNumber: domain.PhoneNumber.Value,
				status: domain.Status.Value,
				version: domain.Version,
				createdAt: domain.CreatedAt.ToUtcStringDateTime()
			);
		}

	}
}
