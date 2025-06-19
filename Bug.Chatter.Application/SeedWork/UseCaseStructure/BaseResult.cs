namespace Bug.Chatter.Application.SeedWork.UseCaseStructure;

public class BaseResult : IResult<IInput>
{
	public BaseResult()
	{
		Status = ResultStatus.Success;
		Reasons = [];
		HasFailed = false;
	}

	public BaseResult(string successMessage) : this(ResultStatus.Success, successMessage) { }

	public BaseResult(ResultStatus status, string reason) : this(status, [reason]) { }

	public BaseResult(ResultStatus status, string[] reasons)
	{
		Status = status;
		Reasons = reasons;
		HasFailed = status != ResultStatus.Success;
	}

	public bool HasFailed { get; private set; }

	public string[] Reasons { get; private set; }

	public ResultStatus Status { get; private set; }

	public void CopyFrom(BaseResult result)
	{
		Status = result.Status;
		Reasons = result.Reasons;
		HasFailed = result.HasFailed;
	}
}

public enum ResultStatus
{
	Failed = 0,
	Success = 1,
	Rejected = 2
}
