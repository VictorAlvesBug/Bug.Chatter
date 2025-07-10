using Bug.Chatter.Domain.Aggregates.Users;
using Bug.Chatter.Domain.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	internal class UserRepository : IUserRepository
	{
		private readonly IDynamoDbRepository<UserDTO> _userContext;
		private readonly string _userSk;

		public UserRepository(IDynamoDbRepository<UserDTO> userContext)
		{
			_userContext = userContext;
			_userSk = DatabaseSettings.UserSk;
		}

		public async Task<User?> GetAsync(UserPk pk)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(pk.Value, nameof(pk));

			var dto = await _userContext.GetAsync(pk.Value, _userSk);
			return dto is not null ? dto.ToDomain() : null;
		}

		public async Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber.Value, nameof(phoneNumber));

			var dtos = await _userContext.ListByIndexKeysAsync(DatabaseSettings.PhoneNumberSkIndex, phoneNumber.Value, _userSk);
			return dtos.FirstOrDefault()?.ToDomain();
		}

		public async Task<User[]> BatchGetAsync(UserPk[] pks)
		{
			ArgumentNullException.ThrowIfNull(pks, nameof(pks));

			var dtos = await _userContext.BatchGetAsync(pks.Select(pk => (pk.Value, _userSk)));

			var missingKeys = MissingKeys(pks, dtos);

			if (missingKeys.Length > 0)
				throw new Exception(
					$"Alguns usuários não foram encontrados. PKs não encontradas: {string.Join(", ", missingKeys)}");

			return dtos.Select(dto => dto.ToDomain()).ToArray();
		}

		public async Task SafePutAsync(User user)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			await _userContext.SafePutAsync(user.ToDTO(_userSk));
		}

		public async Task UpdateAsync(User user, int expectedVersion)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			await _userContext.UpdateDynamicAsync(user.ToDTO(_userSk));
		}

		public async Task DeleteAsync(UserPk pk, int expectedVersion)
		{
			ArgumentException.ThrowIfNullOrEmpty(pk.Value, nameof(pk));

			await _userContext.DeleteAsync(pk.Value, _userSk);
		}

		private static string[] MissingKeys(UserPk[] pks, IEnumerable<UserDTO> dtos)
		{
			return pks.Select(pk => pk.Value).Except(dtos.Where(x => x is not null).Select(x => x.PK)).ToArray();
		}
	}
}
