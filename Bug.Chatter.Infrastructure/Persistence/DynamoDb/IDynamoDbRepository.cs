namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public interface IDynamoDbRepository<T> : IDisposable where T : class
	{
		Task<T?> GetAsync(dynamic pk, dynamic? sk = null, List<string>? attributesToGet = null);
		
		Task<IEnumerable<T>> BatchGetAsync(
			IEnumerable<(dynamic pk, dynamic sk)> keysToGet,
			List<string>? attributesToGet = null);

		Task<IEnumerable<T>> ListByPartitionKeyAsync(
			string pk,
			List<string> attributesToGet);

		Task SafePutAsync(T dto);

		Task<T> UpdateDynamicAsync(T dto);

		Task DeleteAsync(dynamic pk, dynamic? sk = null);


	}
}
