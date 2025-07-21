namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public sealed record DynamoDbIndex
	{
		public string IndexName { get; private set; }
		public string IndexPk { get; private set; }
		public string? IndexSk { get; private set; }

		public DynamoDbIndex(string indexName, string indexPkValue, string indexSkValue)
		{
			ArgumentNullException.ThrowIfNull(indexName, nameof(indexName));
			ArgumentNullException.ThrowIfNull(indexPkValue, nameof(indexPkValue));

			IndexName = indexName;
			IndexPk = indexPkValue;
			IndexSk = indexSkValue;
		}
	}
}
