namespace Bug.Chatter.Domain.SeedWork.ValueObjects
{
	public sealed record UserSk
	{
		public const string Prefix = "user";
		public const string Version = "mainSchema-v0";
		
		public string Value { get; }

		private UserSk(string value)
		{
			Value = value;
		}

		public static UserSk Create() => new($"{Prefix}-{Version}");

		public override string ToString() => Value;
	}
}
