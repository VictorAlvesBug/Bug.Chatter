namespace Bug.Chatter.Domain.SeedWork.ValueObjects
{
	public sealed record UserSk
	{
		public const string Prefix = "user";
		public const string MainSchema = "mainSchema-v0";
		
		public string Value { get; }

		private UserSk(string value)
		{
			Value = value;
		}

		public static UserSk Create() => new($"{Prefix}-{MainSchema}");

		public override string ToString() => Value;
	}
}
