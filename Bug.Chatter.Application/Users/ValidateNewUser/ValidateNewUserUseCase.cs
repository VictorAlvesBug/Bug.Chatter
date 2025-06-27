using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Users.ValidateNew
{
	public class ValidateNewUserUseCase : IUseCase<ValidateNewUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;
		private readonly ICommandMapper<ValidateNewUserCommand, User> _userMapper;

		public ValidateNewUserUseCase(
			IUserRepository userRepository,
			ICommandMapper<ValidateNewUserCommand, User> userMapper)
		{
			_userRepository = userRepository;
			_userMapper = userMapper;
		}

		public async Task<Result<UserModel>> HandleAsync(ValidateNewUserCommand input)
		{
			try
			{
				var user = _userMapper.Map(input);

				var existingUser = await _userRepository.GetByPhoneNumberAsync(user.PhoneNumber);

				if (existingUser != null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.PhoneNumberMustBeUnique, nameof(PhoneNumber), user.PhoneNumber.Value));

				return Result<UserModel>.Success("Usuário válido para cadastro");
			}
			catch (Exception e)
			{
				return Result<UserModel>.Failure(e.Message);
			}
		}
	}
}
