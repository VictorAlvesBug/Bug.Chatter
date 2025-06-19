using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace Bug.Chatter.DataAccess.Repositories.DynamoDb
{
	public interface IDynamoDbTable
	{
		public string TableName { get; }

		public bool TryLoadTable(
			IAmazonDynamoDB ddbClient,
			string tableName,
			out IDynamoDbTable currentTable,
			out string reason);

		public IDocumentBatchGet CreateBatchGet();

		public ISearch Query(QueryOperationConfig queryOperationConfig);

		public Task<Document> GetItemAsync(Primitive hashKey);
		public Task<Document> GetItemAsync(Primitive hashKey, GetItemOperationConfig config);
		public Task<Document> GetItemAsync(Primitive hashKey, Primitive rangeKey);
		public Task<Document> GetItemAsync(Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config);

		public Task<Document> PutItemAsync(Document document);
		public Task<Document> PutItemAsync(Document document, PutItemOperationConfig config);

		public Task<Document> UpdateItemAsync(Document document);
		public Task<Document> UpdateItemAsync(Document document, UpdateItemOperationConfig config);

		public Task<Document> DeleteItemAsync(Document document);
		public Task<Document> DeleteItemAsync(Document document, DeleteItemOperationConfig config);
		public Task<Document> DeleteItemAsync(Primitive hashKey);
		public Task<Document> DeleteItemAsync(Primitive hashKey, DeleteItemOperationConfig config);
		public Task<Document> DeleteItemAsync(Primitive hashKey, Primitive rangeKey);
		public Task<Document> DeleteItemAsync(Primitive hashKey, Primitive rangeKey, DeleteItemOperationConfig config);
	}
}
