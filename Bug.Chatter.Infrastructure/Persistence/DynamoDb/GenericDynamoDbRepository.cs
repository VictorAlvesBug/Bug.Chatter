
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public class GenericDynamoDbRepository<TEntityDTO> : IDynamoDbRepository<TEntityDTO>
		where TEntityDTO : EntityDTO
	{
		protected readonly string TableName;
		protected readonly IAmazonDynamoDB DynamoDbClient;

		protected readonly IDynamoDbTable CurrentTable;

		private readonly IMemoryCache _memoryCache;

		public GenericDynamoDbRepository(
			IAmazonDynamoDB ddbClient,
			IMemoryCache memoryCache)
			: this(ddbClient, new DynamoDbTable(), memoryCache) { }

		public GenericDynamoDbRepository(
			IAmazonDynamoDB ddbClient,
			IDynamoDbTable table,
			IMemoryCache memoryCache)
		{
			DynamoDbClient = ddbClient;
			TableName = DatabaseSettings.ChatterTableName;
			CurrentTable =
				table.TryLoadTable(ddbClient, TableName, out var currentTable, out string reason)
				? currentTable
				: throw new Exception($"Erro ao carregar tabela {TableName}. Motivo: {reason}");
			_memoryCache = memoryCache;
		}

		public async Task<TEntityDTO?> GetAsync(string pk, string sk, List<string>? attributesToGet = null)
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

			return doc?.ConvertTo<TEntityDTO>();
		}

		public async Task<IEnumerable<TEntityDTO>> ListByIndexKeysAsync(string indexName, string indexPkValue, string? indexSkValue = null, List<string>? attributesToGet = null)
		{
			var indexes = await GetIndexAsync(indexName)
				?? throw new Exception($"Nenhum índice '{indexName}' foi encontrado na tabela '{CurrentTable.TableName}'");

			var queryOperationConfig =
				new QueryOperationConfig
				{
					IndexName = indexName,
					Select = SelectValues.AllProjectedAttributes,
					KeyExpression = indexSkValue is null
						?
						new DynamoUtil
						{
							{ "PhoneNumber", indexPkValue }
						}.ToExpression()
						:
						new DynamoUtil
						{
							{ "PhoneNumber", indexPkValue },
							{ "SK", indexSkValue }
						}.ToExpression()
				};

			var keysSearch = CurrentTable.Query(queryOperationConfig);
			var keys = (await keysSearch.GetRemainingAsync()).Select(doc => (doc["PK"].AsString(), doc["SK"].AsString()));

			return await BatchGetAsync(keys, attributesToGet);
		}

		public async Task<IEnumerable<TEntityDTO>> BatchGetAsync(IEnumerable<(string pk, string sk)> keysToGet, List<string>? attributesToGet = null)
		{
			var batchGet = CurrentTable.CreateBatchGet();

			foreach ((var pk, var sk) in keysToGet)
			{
				batchGet.AddKey(new Primitive(pk), new Primitive(sk));
			}

			if (attributesToGet != null)
				batchGet.AttributesToGet = attributesToGet;

			await batchGet.ExecuteAsync();

			return batchGet.Results?.ConvertTo<TEntityDTO>() ?? [];
		}

		public async Task<IEnumerable<TEntityDTO>> ListByPartitionKeyAsync(string pk, List<string>? attributesToGet = null)
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
				.ConvertTo<TEntityDTO>();
		}

		public async Task SafePutAsync(TEntityDTO dto)
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

		public async Task UpdateDynamicAsync(TEntityDTO dto)
		{
			var doc = dto.ToDocument(nullValueHandling: true);

			await CurrentTable.UpdateItemAsync(doc);
		}

		public async Task DeleteAsync(string pk, string sk)
		{
			await CurrentTable.DeleteItemAsync(new Primitive(pk), new Primitive(sk));
		}

		private async Task<DynamoDbIndex?> GetIndexAsync(string indexName)
		{
			var indexes = await _memoryCache.GetOrCreateAsync(CurrentTable.TableName, async entry => {
				var response = await DynamoDbClient.DescribeTableAsync(CurrentTable.TableName);

				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

				return response.Table.GlobalSecondaryIndexes.Select(gsi =>
				{
					var indexPkName = gsi.KeySchema.Find(ks => ks.KeyType == KeyType.HASH)?.AttributeName ?? "PK";
					var indexSkName = gsi.KeySchema.Find(ks => ks.KeyType == KeyType.RANGE)?.AttributeName;

					if (indexSkName is null)
						return new DynamoDbIndex(gsi.IndexName, indexPkName);

					return new DynamoDbIndex(gsi.IndexName, indexPkName, indexSkName);
				});
			});

			return indexes?.FirstOrDefault(i => i.IndexName == indexName);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
