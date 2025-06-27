namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public sealed record DynamoDbIndex
	{
		public string IndexName { get; private set; }
		public string IndexPk { get; private set; }
		public string? IndexSk { get; private set; }

		public DynamoDbIndex(string indexName, string indexPk, string indexSk = null)
		{
			IndexName = indexName ?? throw new ArgumentNullException(indexName, nameof(indexName));
			IndexPk = indexPk ?? throw new ArgumentNullException(indexPk, nameof(indexPk));
			IndexSk = indexSk;
		}
	}
}
