using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Application.Users;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Users.Entities;

namespace Bug.Chatter.Application.Users.RegisterUser
{
	public class RegisterUserUseCase : IUseCase<RegisterUserCommand, Result<UserModel>>
	{
		private readonly IUserRepository _userRepository;

		public RegisterUserUseCase(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<Result<UserModel>> HandleAsync(RegisterUserCommand input)
		{
			try
			{
				var spec = new UserOnlySpecification();

				var user = await _userRepository.GetByUserIdAsync(input.UserId, spec);

				if (user == null)
					return Result<UserModel>.Rejected(string.Format(ErrorReason.User.NotFoundById, nameof(input.UserId), input.UserId));

				user.Register();

				await _userRepository.SaveAsync(user, spec);

				return Result<UserModel>.Success(
					user.ToModel(),
					"Usuário cadastrado com sucesso");
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
