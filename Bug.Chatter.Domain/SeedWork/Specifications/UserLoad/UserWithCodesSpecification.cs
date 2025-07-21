namespace Bug.Chatter.Domain.SeedWork.Specifications.UserLoad
{
	public class UserWithCodesSpecification : IUserLoadSpecification
	{
		public bool IncludeVerificationCodes => true;
	}
}
