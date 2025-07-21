using Bug.Chatter.Domain.Users.Entities;
using Bug.Chatter.Domain.Users.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public static class UserCodeDbMapperExtensions
	{
		public static UserCode ToDomain(this UserCodeDTO dto)
		{
			return new UserCode(
				verificationCode: VerificationCode.Create(dto.VerificationCode),
				status: CodeStatus.From(dto.Status),
				version: dto.Version,
				createdAt: dto.CreatedAt.ToBrazilianDateTime(),
				expiresAt: dto.ExpiresAt.ToBrazilianDateTime()
			);
		}

		public static UserCodeDTO ToDTO(this UserCode domain, User user)
		{
			return new UserCodeDTO(
				pk: KeyFactory.CodePk(user.Id),
				sk: KeyFactory.CodeSk(domain.VerificationCode),
				verificationCode: domain.VerificationCode.Value,
				status: domain.Status.Value,
				version: domain.Version,
				createdAt: domain.CreatedAt.ToUtcStringDateTime(),
				expiresAt: domain.ExpiresAt.ToUtcStringDateTime(),
				ttl: domain.ExpiresAt.ToUtcTimestamp()
			);
		}

	}
}
