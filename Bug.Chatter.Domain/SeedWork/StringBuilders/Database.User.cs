namespace Bug.Chatter.Domain.SeedWork.StringBuilders;

public static partial class Database
{
	private const string UserPrefix = "user";
	public static string UserSk = Hyphenize(UserPrefix, MainSchemaV0);

	public static string GenerateUserId() => Guid.NewGuid().ToString();
	public static string GetUserPk(string id) => Hyphenize(UserPrefix, id);
}
