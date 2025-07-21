using Bug.Chatter.Application.Users;
using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Application.Users.LoginUser
{
	public class LoginUserUseCase : IUseCase<LoginUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;

		public LoginUserUseCase(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Result<UserModel>> HandleAsync(LoginUserCommand input)
		{
			try
			{
				var spec = new UserOnlySpecification();

				var user = await _userRepository.GetByPhoneNumberAsync(input.PhoneNumber, spec);

				if (user == null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.NotFoundByPhoneNumber, nameof(PhoneNumber), input.PhoneNumber.Value));

				// TODO: Criar e gerenciar token de acesso

				return Result<UserModel>.Success(
					user.ToModel(),
					"Usuário logado com sucesso");
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
