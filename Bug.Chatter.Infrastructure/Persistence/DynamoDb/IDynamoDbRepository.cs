namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public interface IDynamoDbRepository<TEntityDTO> : IDisposable 
		where TEntityDTO : EntityDTO
	{
		Task<TEntityDTO?> GetAsync(string pk, string sk, List<string>? attributesToGet = null);

		Task<IEnumerable<TEntityDTO>> ListByIndexKeysAsync(string indexName, string indexPkValue, string? indexSkValue = null, List<string>? attributesToGet = null);

		Task<IEnumerable<TEntityDTO>> BatchGetAsync(
			IEnumerable<(string pk, string sk)> keysToGet,
			List<string>? attributesToGet = null);

		Task<IEnumerable<TEntityDTO>> ListByPartitionKeyAsync(
			string pk,
			List<string>? attributesToGet = null);

		Task SafePutAsync(TEntityDTO dto);

		Task UpdateDynamicAsync(TEntityDTO dto);

		Task DeleteAsync(string pk, string sk);


	}
}
