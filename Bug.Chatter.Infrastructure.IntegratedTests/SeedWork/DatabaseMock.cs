using Bug.Chatter.Infrastructure.Persistence.DynamoDb;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users;

namespace Bug.Chatter.Infrastructure.IntegratedTests.SeedWork
{
	public class DatabaseMock
	{
		private readonly Dictionary<Type, object> _store = new()
		{
			{ typeof(UserDTO), new Dictionary<(string pk, string sk), UserDTO>() },
			{ typeof(CodeDTO), new Dictionary<(string pk, string sk), CodeDTO>() }
		};

		public DatabaseMock()
		{
			UseDefault();
		}

		public Dictionary<(string pk, string sk), TEntityDTO> GetMock<TEntityDTO>()
			where TEntityDTO : EntityDTO
		{
			if (_store.TryGetValue(typeof(TEntityDTO), out var dictObj)
				&& dictObj is Dictionary<(string pk, string sk), TEntityDTO> typedDict)
			{
				return typedDict;
			}

			throw new NotSupportedException($"Tipo não suportado: {typeof(TEntityDTO).Name}");
		}

		public void Setup<TEntityDTO>(IEnumerable<TEntityDTO> list)
			where TEntityDTO : EntityDTO
		{
			if (_store.TryGetValue(typeof(TEntityDTO), out var dictObj)
				&& dictObj is Dictionary<(string pk, string sk), TEntityDTO> typedDict)
			{
				_store[typeof(TEntityDTO)] = list.ToDictionary(entity => (entity.PK, entity.SK));
			}
		}

		public void UseDefault()
		{
			UseDefaultUsers();
			UseDefaultCodes();
		}

		public void UseDefaultUsers()
		{
			IEnumerable<UserDTO> defaultValues = [
				new(
					pk: "user-094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					sk: "user-mainSchema-v0",
					id: "094b1c2d-ee50-4c68-a18a-8dca65d450c6",
					name: "Victor Bugueno",
					phoneNumber: "+55 (11) 97562-3736",
					version: 999,
					createdAt: "2025-06-27T00:00:00"),
				new(
					pk: "user-ea9983c8-be00-4307-93ad-635d961de718",
					sk: "user-mainSchema-v0",
					id: "ea9983c8-be00-4307-93ad-635d961de718",
					name: "Fatima Alves",
					phoneNumber: "+55 (11) 98237-5687",
					version: 999,
					createdAt: "2025-06-27T00:00:00")
			];

			Setup(defaultValues);
		}

		public void UseDefaultCodes()
		{
			IEnumerable<CodeDTO> defaultValues = [
				new(
					pk: "code-123456",
					sk: "code-mainSchema-v0",
					numericCode: "123456",
					phoneNumber: "+55 (11) 97562-3736",
					status: "Sent",
					version: 999,
					createdAt: "2099-01-10T01:05:00",
					expiresAt: "2099-01-10T01:15:00",
					ttl: 4072448700),
				new(
					pk: "code-654321",
					sk: "code-mainSchema-v0",
					numericCode: "654321",
					phoneNumber: "+55 (11) 98237-5687",
					status: "NotSentYet",
					version: 999,
					createdAt: "2099-01-10T01:05:00",
					expiresAt: "2099-01-10T01:15:00",
					ttl: 4072449300)
			];

			Setup(defaultValues);
		}
	}
}
