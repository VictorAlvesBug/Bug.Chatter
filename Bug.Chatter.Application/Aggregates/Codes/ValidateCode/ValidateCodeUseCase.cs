using Bug.Chatter.Application.Aggregates.Codes.SendNewCode;
using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug.Chatter.Application.Aggregates.Codes.ValidateCode
{
	internal class ValidateCodeUseCase : IUseCase<ValidateCodeCommand, Result<CodeModel>>
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
				var codePk = CodePk.Create(NumericCode.Create(input.NumericCode));
				var code = await _codeRepository.GetAsync(codePk);

				if (code == null)
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.NotFound, nameof(NumericCode)));

				if (!code.PhoneNumbersMatch(input.PhoneNumber))
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.NotFound, nameof(NumericCode)));

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
