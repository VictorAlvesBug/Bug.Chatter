using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public class DynamoDbTable : IDynamoDbTable
	{
		public string TableName { get; private set; }

		private static readonly Mutex _mutex = new();

		private ITable CurrentTable;

		public bool TryLoadTable(
			IAmazonDynamoDB ddbClient,
			string tableName,
			out IDynamoDbTable table,
			out string reason)
		{
			reason = "";
			_mutex.WaitOne();
			try
			{
				table = new DynamoDbTable
				{
					TableName = tableName,
					CurrentTable = Table.LoadTable(ddbClient, tableName)
				};

				return true;
			}
			catch (Exception e)
			{
				table = null;
				reason = e.Message;
				return false;
			}
			finally
			{
				_mutex.ReleaseMutex();
			}
		}

		public IDocumentBatchGet CreateBatchGet()
			=> CurrentTable.CreateBatchGet();

		public ISearch Query(QueryOperationConfig queryOperationConfig)
			=> CurrentTable.Query(queryOperationConfig);

		public Task<Document> GetItemAsync(Primitive hashKey)
			=> CurrentTable.GetItemAsync(hashKey);

		public Task<Document> GetItemAsync(Primitive hashKey, GetItemOperationConfig config)
			=> CurrentTable.GetItemAsync(hashKey, config);

		public Task<Document> GetItemAsync(Primitive hashKey, Primitive rangeKey)
			=> CurrentTable.GetItemAsync(hashKey, rangeKey);

		public Task<Document> GetItemAsync(Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config)
			=> CurrentTable.GetItemAsync(hashKey, rangeKey, config);

		public Task<Document> PutItemAsync(Document document)
			=> CurrentTable.PutItemAsync(document);

		public Task<Document> PutItemAsync(Document document, PutItemOperationConfig config)
			=> CurrentTable.PutItemAsync(document, config);

		public Task<Document> UpdateItemAsync(Document document)
			=> CurrentTable.UpdateItemAsync(document);

		public Task<Document> UpdateItemAsync(Document document, UpdateItemOperationConfig config)
			=> CurrentTable.UpdateItemAsync(document, config);

		public Task<Document> DeleteItemAsync(Document document)
			=> CurrentTable.DeleteItemAsync(document);

		public Task<Document> DeleteItemAsync(Document document, DeleteItemOperationConfig config)
			=> CurrentTable.DeleteItemAsync(document, config);

		public Task<Document> DeleteItemAsync(Primitive hashKey)
			=> CurrentTable.DeleteItemAsync(hashKey);

		public Task<Document> DeleteItemAsync(Primitive hashKey, DeleteItemOperationConfig config)
			=> CurrentTable.DeleteItemAsync(hashKey, config);

		public Task<Document> DeleteItemAsync(Primitive hashKey, Primitive rangeKey)
			=> CurrentTable.DeleteItemAsync(hashKey, rangeKey);

		public Task<Document> DeleteItemAsync(Primitive hashKey, Primitive rangeKey, DeleteItemOperationConfig config)
			=> CurrentTable.DeleteItemAsync(hashKey, rangeKey, config);
	}
}
