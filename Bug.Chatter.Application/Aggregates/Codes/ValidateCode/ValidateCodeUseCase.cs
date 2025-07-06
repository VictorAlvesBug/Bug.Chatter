using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Aggregates.Codes.ValidateCode
{
	public class ValidateCodeUseCase : IUseCase<ValidateCodeCommand, Result<CodeModel>>
	{
		private readonly ICodeRepository _codeRepository;

		public ValidateCodeUseCase(
			ICodeRepository codeRepository)
		{
			_codeRepository = codeRepository;
		}

		public async Task<Result<CodeModel>> HandleAsync(ValidateCodeCommand input)
		{
			try
			{
				var phoneNumber = PhoneNumber.Create(input.PhoneNumber);
				var numericCode = NumericCode.Create(input.NumericCode);
				var codePk = CodePk.Create(numericCode);

				var code = await _codeRepository.GetAsync(codePk);

				if (code == null)
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.NotFound, nameof(NumericCode), numericCode.Value));

				if (!code.PhoneNumbersMatch(phoneNumber))
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.NotFound, nameof(NumericCode), numericCode.Value));

				if (code.IsExpired())
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.Expired, nameof(NumericCode)));

				return Result<CodeModel>.Success("Código de verificação validado");
			}
			catch (Exception e)
			{
				return Result<CodeModel>.Failure(e.Message);
			}
		}
	}
}
