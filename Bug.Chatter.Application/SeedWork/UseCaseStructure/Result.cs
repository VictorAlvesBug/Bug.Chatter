namespace Bug.Chatter.Application.SeedWork.UseCaseStructure
{
	public enum ResultStatus
	{
		Failure = 0,
		Success = 1,
		Rejected = 2
	}

	public class Result<TData>
	{
		public ResultStatus Status { get; }
		public string[] Reasons { get; }
		public TData? Data { get; }

		private Result(ResultStatus status, string[] reasons, TData? data = default)
		{
			Status = status;
			Reasons = reasons;
			Data = data;
		}

		public static Result<TData> Success(TData data, params string[] reasons)
			=> new(ResultStatus.Success, reasons, data);

		public static Result<TData> Success(params string[] reasons)
			=> new(ResultStatus.Success, reasons);

		public static Result<TData> Failure(TData data, params string[] reasons)
			=> new(ResultStatus.Failure, reasons, data);

		public static Result<TData> Failure(params string[] reasons)
			=> new(ResultStatus.Failure, reasons);

		public static Result<TData> Rejected(TData data, params string[] reasons)
			=> new(ResultStatus.Rejected, reasons, data);

		public static Result<TData> Rejected(params string[] reasons)
			=> new(ResultStatus.Rejected, reasons);
	}
}
