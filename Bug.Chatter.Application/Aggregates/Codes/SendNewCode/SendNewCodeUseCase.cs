using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Aggregates.Codes;
using Bug.Chatter.Domain.Errors;
using Polly;

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
				var retryPolicy = Policy
					.HandleResult<(Code newCode, bool codeAlreadyExists)>(
						result => result.codeAlreadyExists
					)
					.Retry(
						retryCount: 10,
						onRetry: (result, retryCount) =>
						{
							var newCodeValue = result.Result.newCode.NumericCode.Value;

							Console.WriteLine(string.Format(ErrorReason.Code.GenerateCodeRetry, retryCount, newCodeValue));
						});

				(Code newCode, bool codeAlreadyExists) = retryPolicy.Execute(() =>
				{
					newCode = _codeMapper.Map(input);

					var existingCode = _codeRepository.GetAsync(newCode.Pk)
						.GetAwaiter()
						.GetResult();

					return (newCode, existingCode is not null);
				});

				if (codeAlreadyExists)
					return Result<CodeModel>.Rejected(string.Format(ErrorReason.Code.MaxAttemptsToGenerateCodeReached));

				await _codeRepository.SafePutAsync(newCode);

				return Result<CodeModel>.Success("Um código de verificação será enviado via SMS");
			}
			catch (Exception e)
			{
				return Result<CodeModel>.Failure(e.Message);
			}
		}
	}
}
