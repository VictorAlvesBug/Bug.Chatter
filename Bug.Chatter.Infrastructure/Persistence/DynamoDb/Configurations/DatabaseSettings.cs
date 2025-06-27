namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations
{
	public static class DatabaseSettings
	{
		public const string ChatterTableName = "bug-chatter";
		public const string DatabaseDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
		public const string FrontendDateTimeFormat = "dd/MM/yyyy - HH:mm:ss";
		public const string UserSk = "user-mainSchema-v0";
		public const string UserPhoneNumberSkIndex = "PhoneNumber-SK-index";
	}
}
