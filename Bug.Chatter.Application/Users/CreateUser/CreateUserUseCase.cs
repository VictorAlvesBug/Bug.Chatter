using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Application.Users.CreateUser
{
	public class CreateUserUseCase : IUseCase<CreateUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;
		private readonly ICommandMapper<CreateUserCommand, User> _userMapper;

		public CreateUserUseCase(
			IUserRepository userRepository,
			ICommandMapper<CreateUserCommand, User> userMapper)
		{
			_userRepository = userRepository;
			_userMapper = userMapper;
		}

		public async Task<Result<UserModel>> HandleAsync(CreateUserCommand input)
		{
			try
			{
				var user = _userMapper.Map(input);
				
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
