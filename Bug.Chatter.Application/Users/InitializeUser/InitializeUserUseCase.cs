using Bug.Chatter.Application.Users;
using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users.Entities;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Application.Users.InitializeUser
{
	public class InitializeUserUseCase : IUseCase<InitializeUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;
		private readonly ICommandMapper<InitializeUserCommand, User> _userMapper;

		public InitializeUserUseCase(
			IUserRepository userRepository,
			ICommandMapper<InitializeUserCommand, User> userMapper)
		{
			_userRepository = userRepository;
			_userMapper = userMapper;
		}

		public async Task<Result<UserModel>> HandleAsync(InitializeUserCommand input)
		{
			try
			{
				var spec = new UserOnlySpecification();

				var user = _userMapper.Map(input);

				var existingUser = await _userRepository.GetByPhoneNumberAsync(user.PhoneNumber, spec);

				if (existingUser == null)
				{
					await _userRepository.SaveAsync(user, spec);
					return Result<UserModel>.Success(user.ToModel(), "Usuário pré-cadastrado com sucesso");
				}

				if (User.StatusAllowInitializeUser(existingUser.Status))
				{
					existingUser.UpdateName(user.Name);
					existingUser.UpdateStatus(user.Status);
					await _userRepository.SaveAsync(existingUser, spec);
					return Result<UserModel>.Success(user.ToModel(), "Dados atualizados com sucesso para usuário pré-cadastrado");
				}
				
				return Result<UserModel>.Rejected(string.Format(ErrorReason.User.PhoneNumberMustBeUnique, nameof(PhoneNumber), user.PhoneNumber.Value));
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
