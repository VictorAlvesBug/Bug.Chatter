using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Application.Users
{
	public class CreateUserUseCase : IUseCase<CreateUserCommand, CreateUserResult>
	{
		private readonly IUserRepository _repository;

		public CreateUserUseCase(IUserRepository repository)
		{
			_repository = repository;
		}

		public async Task<CreateUserResult> HandleAsync(CreateUserCommand input)
		{
			try
			{
				var user = User.CreateNew(input.Name, input.PhoneNumber);

				await _repository.SafePutAsync(user);

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
