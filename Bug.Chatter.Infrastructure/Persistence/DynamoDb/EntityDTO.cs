namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public class EntityDTO(string pk, string sk)
	{
		public string PK { get; } = pk;
		public string SK { get; } = sk;
	}
}
