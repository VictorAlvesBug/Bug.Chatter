using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.SeedWork
{
	public static class TestingHelper
	{
		public static void OverrideWithMockContext<TContext, TEntityDTO>(
			this ServiceCollection services,
			DatabaseMock databaseMock,
			Mock<TContext> mockContext
		)
			where TEntityDTO : EntityDTO
			where TContext : class, IDynamoDbRepository<TEntityDTO>
		{
			mockContext
				.Setup(mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string pk, string sk, List<string> _) =>
				{
					return databaseMock.GetMock<TEntityDTO>().GetValueOrDefault((pk, sk));
				});

			mockContext
				.Setup(mock => mock.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string indexName, string indexPkValue, string indexSkValue, List<string> _) =>
				{
					var parts = indexName.Split('-');
					var indexPkName = parts[0];
					var indexSkName = parts[1];

					var list = databaseMock.GetMock<TEntityDTO>().Select(kvp => kvp.Value);

					return list.Where(entity =>
					{
						var entityIndexPkValue = entity.GetType().GetProperty(indexPkName)?.GetValue(entity)?.ToString();
						var entityIndexSkValue = entity.GetType().GetProperty(indexSkName)?.GetValue(entity)?.ToString();

						if (indexSkValue is null)
							return entityIndexPkValue == indexPkValue;

						return entityIndexPkValue == indexPkValue && entityIndexSkValue == indexSkValue;
					});
				});

			mockContext
				.Setup(mock => mock.BatchGetAsync(It.IsAny<IEnumerable<(string, string)>>(), It.IsAny<List<string>>()))
				.ReturnsAsync((IEnumerable<(string pk, string sk)> keysToGet, List<string> _) =>
					keysToGet
						.Select(keyPair => databaseMock.GetMock<TEntityDTO>().GetValueOrDefault((keyPair.pk, keyPair.sk)))
						.Where(entity => entity is not null)
						.Select(entity => entity!)
				);

			mockContext
				.Setup(mock => mock.ListByPartitionKeyAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string pk, List<string> _) =>
					databaseMock.GetMock<TEntityDTO>().Select(kvp => kvp.Value).Where(entity => entity.PK == pk)
				);

			mockContext
				.Setup(mock => mock.SafePutAsync(It.IsAny<TEntityDTO>()))
				.Callback((TEntityDTO dto) =>
				{
					var alreadyExists = databaseMock.GetMock<TEntityDTO>().ContainsKey((dto.PK, dto.SK));

					if (alreadyExists)
						throw new Exception($"Já existe um documento com as mesmas chaves. PK: {dto.PK} - SK: {dto.SK}");

					databaseMock.GetMock<TEntityDTO>().Add((dto.PK, dto.SK), dto);
				});

			mockContext
				.Setup(mock => mock.UpdateDynamicAsync(It.IsAny<TEntityDTO>()))
				.Callback((TEntityDTO entityUpdated) =>
				{
					var entityFromDb = databaseMock.GetMock<TEntityDTO>().GetValueOrDefault((entityUpdated.PK, entityUpdated.SK));

					if (entityFromDb is null)
						throw new Exception($"Documento não encontrado para ser atualizado. PK: {entityUpdated.PK} - SK: {entityUpdated.SK}");

					databaseMock.GetMock<TEntityDTO>().Remove((entityUpdated.PK, entityUpdated.SK));

					var objFromDb = JObject.Parse(entityFromDb.ToJson());
					var objUpdated = JObject.Parse(entityUpdated.ToJson(nullValueHandling: true));

					objFromDb.Merge(objUpdated, new JsonMergeSettings
					{
						MergeArrayHandling = MergeArrayHandling.Replace,
						MergeNullValueHandling = MergeNullValueHandling.Ignore
					});

					var mergedEntity = objFromDb.ToString().DeserializeJson<TEntityDTO>();

					if (mergedEntity is null)
						throw new Exception($"Erro ao deserializar objeto. PK: {entityUpdated.PK} - SK: {entityUpdated.SK}");

					databaseMock.GetMock<TEntityDTO>().Add((mergedEntity.PK, mergedEntity.SK), mergedEntity);
				});

			mockContext
				.Setup(mock => mock.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
				.Callback((string pk, string sk) => {
					var exists = databaseMock.GetMock<TEntityDTO>().ContainsKey((pk, sk));

					if (!exists)
						throw new Exception($"Documento não encontrado para ser deletado. PK: {pk} - SK: {sk}");

					databaseMock.GetMock<TEntityDTO>().Remove((pk, sk));
				});


			services.AddScoped(_ => mockContext.Object);
		}
	}
}
