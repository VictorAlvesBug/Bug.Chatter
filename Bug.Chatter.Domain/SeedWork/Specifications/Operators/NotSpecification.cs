namespace Bug.Chatter.Domain.SeedWork.Specifications.Operators
{
	public class NotSpecification<T> : Specification<T> where T : class
	{
		private readonly ISpecification<T> _spec;

		public NotSpecification(ISpecification<T> spec)
		{
			_spec = spec;
		}

		public override bool IsSatisfiedBy(T candidate)
		{
			return !_spec.IsSatisfiedBy(candidate);
		}
	}
}
