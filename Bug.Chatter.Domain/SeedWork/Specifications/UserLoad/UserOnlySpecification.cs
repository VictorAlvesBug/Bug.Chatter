namespace Bug.Chatter.Domain.SeedWork.Specifications.UserLoad
{
	public class UserOnlySpecification : IUserLoadSpecification
	{
		public bool IncludeVerificationCodes => false;
	}
}
