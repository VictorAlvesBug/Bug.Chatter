namespace Bug.Chatter.Application.SeedWork.UseCaseStructure;

public interface IUseCase<TInput, TResult> where TInput : IInput where TResult : IResult<TInput>
{
	public Task<TResult> HandleAsync(TInput input);
}
