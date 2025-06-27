using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Users.RegisterUser
{
	public class RegisterUserUseCase : IUseCase<RegisterUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;
		private readonly ICommandMapper<RegisterUserCommand, User> _userMapper;

		public RegisterUserUseCase(
			IUserRepository userRepository,
			ICommandMapper<RegisterUserCommand, User> userMapper)
		{
			_userRepository = userRepository;
			_userMapper = userMapper;
		}

		public async Task<Result<UserModel>> HandleAsync(RegisterUserCommand input)
		{
			try
			{
				var user = _userMapper.Map(input);

				var existingUser = await _userRepository.GetByPhoneNumberAsync(user.PhoneNumber);

				if (existingUser != null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.PhoneNumberMustBeUnique, nameof(PhoneNumber), user.PhoneNumber.Value));

				await _userRepository.SafePutAsync(user);

				return Result<UserModel>.Success(
					user.ToModel(),
					"Usuário cadastrado com sucesso");
			}
			catch (Exception e)
			{
				return Result<UserModel>.Failure(e.Message);
			}
		}
	}
}
