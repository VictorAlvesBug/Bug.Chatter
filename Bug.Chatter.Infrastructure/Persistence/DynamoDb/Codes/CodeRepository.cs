using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes
{
	internal class CodeRepository : ICodeRepository
	{
		private readonly ICodeContext _codeContext;
		private readonly string _codeSk;

		public CodeRepository(ICodeContext codeContext)
		{
			_codeContext = codeContext;
			_codeSk = DatabaseSettings.CodeSk;
		}

		public async Task<Code?> GetAsync(CodePk pk)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(pk.Value, nameof(pk));

			var dto = await _codeContext.GetAsync(pk.Value, _codeSk);
			return dto is not null ? dto.ToDomain() : null;
		}

		public async Task SafePutAsync(Code code)
		{
			ArgumentNullException.ThrowIfNull(code, nameof(code));

			await _codeContext.SafePutAsync(code.ToDTO(_codeSk));
		}

		public async Task<IEnumerable<Code>> ListByPhoneNumberAsync(PhoneNumber phoneNumber)
		{
			ArgumentNullException.ThrowIfNullOrWhiteSpace(phoneNumber.Value, nameof(phoneNumber));

			var dtos = await _codeContext.ListByIndexKeysAsync(DatabaseSettings.PhoneNumberSkIndex, phoneNumber.Value, _codeSk);
			return dtos.Select(dto => dto.ToDomain());
		}
	}
}
