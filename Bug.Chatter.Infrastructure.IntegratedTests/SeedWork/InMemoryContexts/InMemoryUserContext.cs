using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;
using Newtonsoft.Json.Linq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.SeedWork.InMemoryContexts
{
	public class InMemoryUserContext : IDynamoDbRepository<UserDTO>
	{
		private readonly Dictionary<(string pk, string sk), UserDTO> _userDtos = new Dictionary<(string pk, string sk), UserDTO>();

		public void UseDefaultValues()
		{
			_userDtos.Clear();

			((List<UserDTO>)[
				new(
					pk: "user-094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					sk: "user-mainSchema-v0",
					id: "094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					name: "Victor Bugueno",
					phoneNumber: "+55 (11) 97562-3736",
					status: "Registered",
					version: 999,
					createdAt: "2025-06-27T00:00:00"),
				new(
					pk: "user-ea9983c8-be00-4307-93ad-635d961de718",
					sk: "user-mainSchema-v0",
					id: "ea9983c8-be00-4307-93ad-635d961de718",
					name: "Fatima Alves",
					phoneNumber: "+55 (11) 98237-5687",
					status: "Draft",
					version: 999,
					createdAt: "2025-06-27T00:00:00")
			])
			.ForEach(user => _userDtos[(user.PK, user.SK)] = user);
		}

		public Task<UserDTO?> GetAsync(string pk, string sk, List<string>? attributesToGet = null)
		{
			return Task.FromResult(_userDtos.GetValueOrDefault((pk, sk)));
		}

		public Task<IEnumerable<UserDTO>> BatchGetAsync(IEnumerable<(string pk, string sk)> keysToGet, List<string>? attributesToGet = null)
		{
			var result = keysToGet
				.Select(keyPair => _userDtos.GetValueOrDefault((keyPair.pk, keyPair.sk)))
				.Where(entity => entity is not null)
				.Select(entity => entity!);

			return Task.FromResult(result);
		}

		public Task<IEnumerable<UserDTO>> ListByIndexKeysAsync(string indexName, string indexPkValue, string? indexSkValue = null, List<string>? attributesToGet = null)
		{
			var parts = indexName.Split('-');
			var indexPkName = parts[0];
			var indexSkName = parts[1];

			var list = _userDtos.Select(kvp => kvp.Value);

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

		public Task<IEnumerable<UserDTO>> ListByPartitionKeyAsync(string pk, List<string>? attributesToGet = null)
		{
			return Task.FromResult(_userDtos.Select(kvp => kvp.Value).Where(entity => entity.PK == pk));
		}

		public Task<IEnumerable<UserDTO>> QueryAsync(string pk, string skBeginsWith, List<string>? attributesToGet = null)
		{
			var result = _userDtos.Select(kvp => kvp.Value).Where(entity => entity.PK == pk && entity.SK.StartsWith(skBeginsWith));

			return Task.FromResult(result);
		}

		public Task SafePutAsync(UserDTO dto)
		{
			var alreadyExists = _userDtos.ContainsKey((dto.PK, dto.SK));

			if (alreadyExists)
				throw new Exception($"Já existe um documento com as mesmas chaves. PK: {dto.PK} - SK: {dto.SK}");

			_userDtos[(dto.PK, dto.SK)] = dto;

			return Task.CompletedTask;
		}

		public Task UpdateDynamicAsync(UserDTO entityUpdated, int expectedVersion)
		{
			var entityFromDb = _userDtos.GetValueOrDefault((entityUpdated.PK, entityUpdated.SK));

			if (entityFromDb is null)
				throw new Exception($"Documento não encontrado para ser atualizado. PK: {entityUpdated.PK} - SK: {entityUpdated.SK}");

			_userDtos.Remove((entityUpdated.PK, entityUpdated.SK));

			if (entityFromDb.Version != expectedVersion)
				throw new Exception($"Versão do documento persistido não compatível com a versão esperada. PK: {entityUpdated.PK} - SK: {entityUpdated.SK} - Version: {entityFromDb.Version} - ExpectedVersion: {expectedVersion}");

			var objFromDb = JObject.Parse(entityFromDb.ToJson());
			var objUpdated = JObject.Parse(entityUpdated.ToJson(nullValueHandling: true));

			objFromDb.Merge(objUpdated, new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Replace,
				MergeNullValueHandling = MergeNullValueHandling.Ignore
			});

			var mergedEntity = objFromDb.ToString().DeserializeJson<UserDTO>();

			if (mergedEntity is null)
				throw new Exception($"Erro ao deserializar objeto. PK: {entityUpdated.PK} - SK: {entityUpdated.SK}");

			_userDtos.Add((mergedEntity.PK, mergedEntity.SK), mergedEntity);

			return Task.CompletedTask;
		}

		public Task DeleteAsync(string pk, string sk, int expectedVersion)
		{
			var entityFromDb = _userDtos.GetValueOrDefault((pk, sk)) ?? throw new Exception($"Documento não encontrado para ser deletado. PK: {pk} - SK: {sk}");

			if (entityFromDb.Version != expectedVersion)
				throw new Exception($"Versão do documento persistido não compatível com a versão esperada. PK: {pk} - SK: {sk} - Version: {entityFromDb.Version} - ExpectedVersion: {expectedVersion}");

			_userDtos.Remove((pk, sk));

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
