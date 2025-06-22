namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public interface IDynamoDbRepository<T> : IDisposable where T : class
	{
		Task<T?> GetAsync(string pk, string sk, List<string>? attributesToGet = null);
		
		Task<IEnumerable<T>> BatchGetAsync(
			IEnumerable<(string pk, string sk)> keysToGet,
			List<string>? attributesToGet = null);

		Task<IEnumerable<T>> ListByPartitionKeyAsync(
			string pk,
			List<string>? attributesToGet = null);

		Task SafePutAsync(T dto);

		Task UpdateDynamicAsync(T dto);

		Task DeleteAsync(string pk, string sk);


	}
}
