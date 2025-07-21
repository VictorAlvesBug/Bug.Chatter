using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Newtonsoft.Json.Linq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.SeedWork.InMemoryContexts
{
	public class InMemoryUserCodeContext : IDynamoDbRepository<UserCodeDTO>
	{
		private readonly Dictionary<(string pk, string sk), UserCodeDTO> _userCodeDtos = new Dictionary<(string pk, string sk), UserCodeDTO>();

		public void UseDefaultValues()
		{
			_userCodeDtos.Clear();

			((List<UserCodeDTO>)[
				new(
					pk: "user-094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					sk: "code-123456",
					verificationCode: "123456",
					status: "Sent",
					version: 999,
					createdAt: "2099-01-10T01:05:00",
					expiresAt: "2099-01-10T01:15:00",
					ttl: 4072448700),
				new(
					pk: "user-ea9983c8-be00-4307-93ad-635d961de718",
					sk: "code-654321",
					verificationCode: "654321",
					status: "NotSentYet",
					version: 999,
					createdAt: "2099-01-10T01:05:00",
					expiresAt: "2099-01-10T01:15:00",
					ttl: 4072449300)
			])
			.ForEach(code => _userCodeDtos[(code.PK, code.SK)] = code);
		}

		public Task<UserCodeDTO?> GetAsync(string pk, string sk, List<string>? attributesToGet = null)
		{
			return Task.FromResult(_userCodeDtos.GetValueOrDefault((pk, sk)));
		}

		public Task<IEnumerable<UserCodeDTO>> BatchGetAsync(IEnumerable<(string pk, string sk)> keysToGet, List<string>? attributesToGet = null)
		{
			var result = keysToGet
				.Select(keyPair => _userCodeDtos.GetValueOrDefault((keyPair.pk, keyPair.sk)))
				.Where(entity => entity is not null)
				.Select(entity => entity!);

			return Task.FromResult(result);
		}

		public Task<IEnumerable<UserCodeDTO>> ListByIndexKeysAsync(string indexName, string indexPkValue, string? indexSkValue = null, List<string>? attributesToGet = null)
		{
			var parts = indexName.Split('-');
			var indexPkName = parts[0];
			var indexSkName = parts[1];

			var list = _userCodeDtos.Select(kvp => kvp.Value);

			var result = list.Where(entity =>
			{
				var entityIndexPkValue = entity.GetType().GetProperty(indexPkName)?.GetValue(entity)?.ToString();
				var entityIndexSkValue = entity.GetType().GetProperty(indexSkName)?.GetValue(entity)?.ToString();

				if (indexSkValue is null)
					return entityIndexPkValue == indexPkValue;

				return entityIndexPkValue == indexPkValue && entityIndexSkValue == indexSkValue;
			});

			return Task.FromResult(result);
		}

		public Task<IEnumerable<UserCodeDTO>> ListByPartitionKeyAsync(string pk, List<string>? attributesToGet = null)
		{
			return Task.FromResult(_userCodeDtos.Select(kvp => kvp.Value).Where(entity => entity.PK == pk));
		}

		public Task<IEnumerable<UserCodeDTO>> QueryAsync(string pk, string skBeginsWith, List<string>? attributesToGet = null)
		{
			var result = _userCodeDtos.Select(kvp => kvp.Value).Where(entity => entity.PK == pk && entity.SK.StartsWith(skBeginsWith));

			return Task.FromResult(result);
		}

		public Task SafePutAsync(UserCodeDTO dto)
		{
			var alreadyExists = _userCodeDtos.ContainsKey((dto.PK, dto.SK));

			if (alreadyExists)
				throw new Exception($"Já existe um documento com as mesmas chaves. PK: {dto.PK} - SK: {dto.SK}");

			_userCodeDtos[(dto.PK, dto.SK)] = dto;

			return Task.CompletedTask;
		}

		public Task UpdateDynamicAsync(UserCodeDTO entityUpdated, int expectedVersion)
		{
			var entityFromDb = _userCodeDtos.GetValueOrDefault((entityUpdated.PK, entityUpdated.SK));

			if (entityFromDb is null)
				throw new Exception($"Documento não encontrado para ser atualizado. PK: {entityUpdated.PK} - SK: {entityUpdated.SK}");

			_userCodeDtos.Remove((entityUpdated.PK, entityUpdated.SK));

			if (entityFromDb.Version != expectedVersion)
				throw new Exception($"Versão do documento persistido não compatível com a versão esperada. PK: {entityUpdated.PK} - SK: {entityUpdated.SK} - Version: {entityFromDb.Version} - ExpectedVersion: {expectedVersion}");

			var objFromDb = JObject.Parse(entityFromDb.ToJson());
			var objUpdated = JObject.Parse(entityUpdated.ToJson(nullValueHandling: true));

			objFromDb.Merge(objUpdated, new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Replace,
				MergeNullValueHandling = MergeNullValueHandling.Ignore
			});

			var mergedEntity = objFromDb.ToString().DeserializeJson<UserCodeDTO>();

			if (mergedEntity is null)
				throw new Exception($"Erro ao deserializar objeto. PK: {entityUpdated.PK} - SK: {entityUpdated.SK}");

			_userCodeDtos.Add((mergedEntity.PK, mergedEntity.SK), mergedEntity);

			return Task.CompletedTask;
		}

		public Task DeleteAsync(string pk, string sk, int expectedVersion)
		{
			var entityFromDb = _userCodeDtos.GetValueOrDefault((pk, sk)) ?? throw new Exception($"Documento não encontrado para ser deletado. PK: {pk} - SK: {sk}");

			if (entityFromDb.Version != expectedVersion)
				throw new Exception($"Versão do documento persistido não compatível com a versão esperada. PK: {pk} - SK: {sk} - Version: {entityFromDb.Version} - ExpectedVersion: {expectedVersion}");

			_userCodeDtos.Remove((pk, sk));

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
