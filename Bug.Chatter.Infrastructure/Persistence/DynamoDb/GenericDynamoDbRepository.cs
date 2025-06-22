
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public abstract class GenericDynamoDbRepository<T> : IDynamoDbRepository<T> where T : class
	{
		protected readonly string TableName;
		protected readonly IAmazonDynamoDB DynamoDbClient;

		protected readonly IDynamoDbTable CurrentTable;

		protected GenericDynamoDbRepository(
			IAmazonDynamoDB ddbClient,
			string tableName)
			: this(ddbClient, new DynamoDbTable(), tableName) { }

		protected GenericDynamoDbRepository(
			IAmazonDynamoDB ddbClient,
			IDynamoDbTable table,
			string tableName)
		{
			DynamoDbClient = ddbClient;
			TableName = tableName;
			CurrentTable =
				table.TryLoadTable(ddbClient, tableName, out var currentTable, out string reason)
				? currentTable
				: throw new Exception($"Erro ao carregar tabela {tableName}. Motivo: {reason}");
		}

		public async Task<T?> GetAsync(string pk, string sk, List<string>? attributesToGet = null)
		{
			Document doc;

			var getItemOperationConfig =
				attributesToGet != null
				? new GetItemOperationConfig
				{
					AttributesToGet = attributesToGet
				}
				: null;

			doc = getItemOperationConfig == null
				? await CurrentTable.GetItemAsync(new Primitive(pk), new Primitive(sk))
				: await CurrentTable.GetItemAsync(new Primitive(pk), new Primitive(sk), getItemOperationConfig);

			return doc?.ConvertTo<T>();
		}

		public async Task<IEnumerable<T>> BatchGetAsync(IEnumerable<(string pk, string sk)> keysToGet, List<string>? attributesToGet = null)
		{
			var batchGet = CurrentTable.CreateBatchGet();

			foreach ((var pk, var sk) in keysToGet)
			{
				batchGet.AddKey(new Primitive(pk), new Primitive(sk));
			}

			if (attributesToGet != null)
				batchGet.AttributesToGet = attributesToGet;

			await batchGet.ExecuteAsync();

			return batchGet.Results?.ConvertTo<T>() ?? [];
		}

		public async Task<IEnumerable<T>> ListByPartitionKeyAsync(string pk, List<string>? attributesToGet = null)
		{
			var queryConfig = new QueryOperationConfig
			{
				Select = attributesToGet is null
					? SelectValues.AllAttributes
					: SelectValues.SpecificAttributes,
				AttributesToGet = attributesToGet,
				KeyExpression = new DynamoUtil
				{
					{ "PK", pk }
				}.ToExpression()
			};

			var search = CurrentTable.Query(queryConfig);

			return (await search.GetRemainingAsync())
				.ConvertTo<T>();
		}

		public async Task SafePutAsync(T dto)
		{
			try
			{
				var document = dto.ToDocument();

				await CurrentTable.PutItemAsync(
					document,
					new PutItemOperationConfig
					{
						ConditionalExpression = new Expression
						{
							ExpressionStatement = "attribute_not_exists(PK) AND attribute_not_exists(SK)"
						}
					}
				);
			}
			catch (Exception e)
			{
				if (e.GetType() == typeof(ConditionalCheckFailedException))
					throw new Exception("Já existe um documento com as mesmas chaves!");

				throw new Exception($"Não foi possível realizar o SafePut: {e.Message}", e);
			}
		}

		public async Task UpdateDynamicAsync(T dto)
		{
			/*var document = dto.ToDocument();

			var request = new UpdateItemRequest
			{
				TableName = CurrentTable.TableName,
				Key = new Dictionary<string, AttributeValue>
				{
					["PK"] = new AttributeValue { S = document["PK"] },
					["SK"] = new AttributeValue { S = document["SK"] },
				},
				ExpressionAttributeNames = new Dictionary<string, string>(),
				ExpressionAttributeValues = new Dictionary<string, AttributeValue>(),
				UpdateExpression = "",
				ReturnValues = ReturnValue.NONE
			};

			var updateParts = new List<string>();

			foreach (var  (key, value) in document)
			{
				if (value is null) continue;

				var attrName = $"#{key}";
				var attrValue = $":{key}";

				//request.ExpressionAttributeNames[attrName] = key;
				//request.ExpressionAttributeValues[attrValue] = value;
				updateParts.Add($"{attrName} = {attrValue}");
			}

			if (!updateParts.Any())
				throw new InvalidOperationException("Nenhum campo válido para atualizar");

			request.UpdateExpression = $"SET {string.Join(", ", updateParts)}";

			await DynamoDbClient.UpdateItemAsync(request);*/

			var doc = dto.ToDocument(nullValueHandling: true);

			await CurrentTable.UpdateItemAsync(doc);
		}

		public async Task DeleteAsync(string pk, string sk)
		{
			await CurrentTable.DeleteItemAsync(new Primitive(pk), new Primitive(sk));
		}

		~GenericDynamoDbRepository() { }

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
