namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public class EntityDTO(string pk, string sk, int version)
	{
		public string PK { get; } = pk;
		public string SK { get; } = sk;
		public int Version { get; } = version;
	}
}
