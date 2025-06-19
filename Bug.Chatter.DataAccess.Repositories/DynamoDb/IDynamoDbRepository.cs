namespace Bug.Chatter.DataAccess.Repositories.DynamoDb
{
	public interface IDynamoDbRepository<T> : IDisposable
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
