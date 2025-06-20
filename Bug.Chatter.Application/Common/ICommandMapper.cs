namespace Bug.Chatter.Application.Common
{
	public interface ICommandMapper<TInput, TDomain>
	{
		TDomain Map(TInput input);
	}
}
