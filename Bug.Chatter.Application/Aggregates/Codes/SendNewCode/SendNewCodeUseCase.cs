using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Application.Aggregates.Codes.SendNewCode
{
	public class SendNewCodeUseCase : IUseCase<SendNewCodeCommand, Result<CodeModel>>
	{
		private readonly ICodeRepository _codeRepository;
		private readonly ICommandMapper<SendNewCodeCommand, Code> _codeMapper;

		public SendNewCodeUseCase(
			ICodeRepository codeRepository,
			ICommandMapper<SendNewCodeCommand, Code> codeMapper)
		{
			_codeRepository = codeRepository;
			_codeMapper = codeMapper;
		}

		public async Task<Result<CodeModel>> HandleAsync(SendNewCodeCommand input)
		{
			try
			{
				Code code;
				Code? existingCode;
				int attemptsCount = 0;
				const int maxAttempts = 20;

				do
				{
					code = _codeMapper.Map(input);
					existingCode = await _codeRepository.GetAsync(code.Pk);
					attemptsCount++;
				}
				while (existingCode is not null && attemptsCount < maxAttempts);

				if(existingCode is not null)
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.MaxAttemptsToGenerateCodeReached));

				await _codeRepository.SafePutAsync(code);

				return Result<CodeModel>.Success("Um código de verificação será enviado via SMS");
			}
			catch (Exception e)
			{
				return Result<CodeModel>.Failure(e.Message);
			}
		}
	}
}
