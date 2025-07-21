namespace Bug.Chatter.Domain.SeedWork.Specifications
{
	public interface ISpecification<T> where T : class
	{
		bool IsSatisfiedBy(T candidate);
		ISpecification<T> And(ISpecification<T> other);
		ISpecification<T> Or(ISpecification<T> other);
		ISpecification<T> Not();
	}
}
