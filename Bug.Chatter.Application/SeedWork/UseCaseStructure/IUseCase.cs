namespace Bug.Chatter.Application.SeedWork.UseCaseStructure;

public interface IUseCase<in TInput, TResult>
{
	Task<TResult> HandleAsync(TInput input);
}
