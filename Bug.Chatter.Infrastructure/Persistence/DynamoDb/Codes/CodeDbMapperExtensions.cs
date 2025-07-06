using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes
{
	public static class CodeDbMapperExtensions
	{
		public static Code ToDomain(this CodeDTO dto)
		{
			return Code.Rehydrate(
				numericCode: NumericCode.Create(dto.NumericCode),
				phoneNumber: PhoneNumber.Create(dto.PhoneNumber),
				status: CodeStatus.From(dto.Status),
				version: dto.Version,
				createdAt: dto.CreatedAt.ToBrazilianDateTime(),
				expiresAt: dto.ExpiresAt.ToBrazilianDateTime()
			);
		}

		public static CodeDTO ToDTO(this Code domain, string codeSk)
		{
			return new CodeDTO(
				pk: domain.Pk.Value,
				sk: codeSk,
				numericCode: domain.NumericCode.Value,
				phoneNumber: domain.PhoneNumber.Value,
				status: domain.Status.Value,
				version: domain.Version,
				createdAt: domain.CreatedAt.ToUtcStringDateTime(),
				expiresAt: domain.ExpiresAt.ToUtcStringDateTime(),
				ttl: domain.ExpiresAt.ToUtcTimestamp()
			);
		}
	}
}
