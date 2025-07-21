using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Users.ValueObjects;
using Bug.Chatter.Domain.Users.Entities;

namespace Bug.Chatter.Application.Users.ValidateVerificationCode
{
	public class ValidateVerificationCodeUseCase : IUseCase<ValidateVerificationCodeCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;

		public ValidateVerificationCodeUseCase(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Result<UserModel>> HandleAsync(ValidateVerificationCodeCommand input)
		{
			try
			{
				var spec = new UserWithCodesSpecification();

				User? user = await _userRepository.GetByUserIdAsync(input.UserId, spec);

				if (user == null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.NotFoundById, nameof(input.UserId), input.UserId));

				if (user.ValidateCode(input.VerificationCode))
				{
					return Result<UserModel>.Success("Código de verificação validado");
				}

				return Result<UserModel>.Rejected(string.Format(ErrorReason.UserCode.Expired, nameof(VerificationCode)));
			}
			catch (Exception e)
			{
				if (e.GetType() == typeof(ArgumentNullException)
					|| e.GetType() == typeof(ArgumentException)
					|| e.GetType() == typeof(InvalidOperationException))
				{
					return Result<UserModel>.Rejected(e.Message);
				}

				return Result<UserModel>.Failure(e.Message);
			}
		}
	}
}
