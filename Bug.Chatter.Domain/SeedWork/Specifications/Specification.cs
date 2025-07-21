using Bug.Chatter.Domain.SeedWork.Specifications.Operators;

namespace Bug.Chatter.Domain.SeedWork.Specifications
{
	public abstract class Specification<T> : ISpecification<T> where T : class
	{
		public abstract bool IsSatisfiedBy(T candidate);

		public ISpecification<T> And(ISpecification<T> other)
			=> new AndSpecification<T>(this, other);

		public ISpecification<T> Or(ISpecification<T> other)
			=> new OrSpecification<T>(this, other);

		public ISpecification<T> Not()
			=> new NotSpecification<T>(this);
	}
}
