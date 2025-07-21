using Bug.Chatter.Domain.Common.ValueObjects;

namespace Bug.Chatter.Application.Users.SendVerificationCode
{
	public class SendVerificationCodeCommand
	{
		public SendVerificationCodeCommand(Guid userId)
		{
			UserId = GuidId.Create(userId);
		}

		public GuidId UserId { get; }
	}
}
