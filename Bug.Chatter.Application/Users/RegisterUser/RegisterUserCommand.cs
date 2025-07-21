using Bug.Chatter.Domain.Common.ValueObjects;

namespace Bug.Chatter.Application.Users.RegisterUser
{
	public class RegisterUserCommand
	{
		public RegisterUserCommand(Guid userId)
		{
			UserId = GuidId.Create(userId);
		}

		public GuidId UserId { get; }
	}
}
