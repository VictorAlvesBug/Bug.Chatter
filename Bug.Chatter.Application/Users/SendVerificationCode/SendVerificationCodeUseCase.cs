using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Application.Users.SendVerificationCode
{
	public class SendVerificationCodeUseCase : IUseCase<SendVerificationCodeCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;

		public SendVerificationCodeUseCase(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Result<UserModel>> HandleAsync(SendVerificationCodeCommand input)
		{
			try
			{
				var spec = new UserWithCodesSpecification();

				var user = await _userRepository.GetByUserIdAsync(input.UserId, spec);
				
				if (user == null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.NotFoundById, nameof(input.UserId), input.UserId));

				user.AddVerificationCode();

				await _userRepository.SaveAsync(user, spec);

				return Result<UserModel>.Success("Um código de verificação será enviado via SMS");
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
