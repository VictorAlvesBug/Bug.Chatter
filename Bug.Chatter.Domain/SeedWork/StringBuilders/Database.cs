namespace Bug.Chatter.Domain.SeedWork.StringBuilders;

public static partial class Database
{
	public const string ChatterTableName = "bug-chatter";
	public const string MainSchemaV0 = "mainSchema-v0";

	public static string Hyphenize(params string[] parts)
	{
		if (parts == null || parts.Length == 0)
			throw new ArgumentException("Ao menos um termo deve ser fornecido para construir uma chave");

		return string.Join("-", parts);
	}
}
