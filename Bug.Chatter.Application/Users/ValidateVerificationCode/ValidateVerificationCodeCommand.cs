using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Application.Users.ValidateVerificationCode
{
	public class ValidateVerificationCodeCommand
	{
		public ValidateVerificationCodeCommand(
			Guid userId,
			string verificationCode)
		{
			UserId = GuidId.Create(userId);
			VerificationCode = VerificationCode.Create(verificationCode);
		}

		public GuidId UserId { get; }
		public VerificationCode VerificationCode { get; }
	}
}
