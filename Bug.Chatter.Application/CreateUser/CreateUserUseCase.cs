using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.CreateUser;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.SeedWork.ValueObjects;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Application.Users
{
	public class CreateUserUseCase : IUseCase<CreateUserCommand, CreateUserResult>
	{
		private readonly IUserRepository _userRepository;
		private readonly ICommandMapper<CreateUserCommand, User> _userMapper;

		public CreateUserUseCase(IUserRepository userRepository, ICommandMapper<CreateUserCommand, User> userMapper)
		{
			_userRepository = userRepository;
			_userMapper = userMapper;
		}

		public async Task<CreateUserResult> HandleAsync(CreateUserCommand input)
		{
			try
			{
				var user = _userMapper.Map(input);
				await _userRepository.SafePutAsync(user);

				return new CreateUserResult(
					ResultStatus.Success,
					"Usuário cadastrado com sucesso",
					user.ToModel());
			}
			catch (Exception e)
			{
				return new CreateUserResult(
					ResultStatus.Failed,
					e.Message);
			}
		}
	}
}
