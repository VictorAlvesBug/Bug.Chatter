using Bug.Chatter.Application.SeedWork.UseCaseStructure;
using Bug.Chatter.Domain.Models;

namespace Bug.Chatter.Application.Users
{
	public class CreateUserResult : BaseResult, IResult<CreateUserCommand>
	{
		public CreateUserResult(ResultStatus status, string reason) : base(status, reason)
		{
		}
		public CreateUserResult(ResultStatus status, string reason, UserModel data) : base(status, reason)
		{
			Data = data;
		}

		public UserModel Data { get; }
	}
}
