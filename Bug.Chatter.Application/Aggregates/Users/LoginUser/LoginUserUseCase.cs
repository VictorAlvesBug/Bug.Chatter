using Bug.Chatter.Application.Aggregates.Users;
using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Aggregates.Users;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Aggregates.Users.LoginUser
{
	public class LoginUserUseCase : IUseCase<LoginUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;

		public LoginUserUseCase(
			IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Result<UserModel>> HandleAsync(LoginUserCommand input)
		{
			try
			{
				var phoneNumber = PhoneNumber.Create(input.PhoneNumber);
				var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

				if (user == null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.NotFound, nameof(PhoneNumber), phoneNumber.Value));

				return Result<UserModel>.Success(
					user.ToModel(),
					"Usuário logado com sucesso");
			}
			catch (Exception e)
			{
				return Result<UserModel>.Failure(e.Message);
			}
		}
	}
}
