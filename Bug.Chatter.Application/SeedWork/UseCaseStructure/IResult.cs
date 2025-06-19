namespace Bug.Chatter.Application.SeedWork.UseCaseStructure;

public interface IResult<TInput> where TInput : IInput
{
	public bool HasFailed { get; }

	public string[] Reasons { get; }
}