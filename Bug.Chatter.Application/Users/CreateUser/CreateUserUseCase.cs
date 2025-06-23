using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.EventStores;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Application.Users.CreateUser
{
	public class CreateUserUseCase : IUseCase<CreateUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;
		private readonly IEventStoreRepository<User> _userEventStoreRepository;
		private readonly ICommandMapper<CreateUserCommand, User> _userMapper;

		public CreateUserUseCase(
			IUserRepository userRepository, 
			IEventStoreRepository<User> userEventStoreRepository,
			ICommandMapper<CreateUserCommand, User> userMapper)
		{
			_userRepository = userRepository;
			_userEventStoreRepository = userEventStoreRepository;
			_userMapper = userMapper;
		}

		public async Task<Result<UserModel>> HandleAsync(CreateUserCommand input)
		{
			try
			{
				var user = _userMapper.Map(input);
				
				await _userRepository.SafePutAsync(user);
				await _userEventStoreRepository.AppendAsync(user.DomainEvents);

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
