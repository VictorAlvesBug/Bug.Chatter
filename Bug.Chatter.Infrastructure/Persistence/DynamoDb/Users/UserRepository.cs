using Bug.Chatter.Domain.Users;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users.Mappers;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	internal class UserRepository : IUserRepository
	{
		private readonly IUserContext _userContext;
		private readonly string _userSk;

		public UserRepository(
			IUserContext userContext)
		{
			_userContext = userContext;
			_userSk = DatabaseSettings.UserSk;
		}

		public async Task<User?> GetAsync(string pk)
		{
			ArgumentException.ThrowIfNullOrEmpty(pk, nameof(pk));

			var dto = await _userContext.GetAsync(pk, _userSk);
			return dto is not null ? dto.ToDomain() : null;
		}

		public async Task<User[]> BatchGetAsync(string[] pks)
		{
			ArgumentNullException.ThrowIfNull(pks, nameof(pks));
			
			var dtos = await _userContext.BatchGetAsync(pks.Select(pk => ((dynamic) pk, (dynamic)_userSk)));

			var missingKeys = MissingKeys(pks, dtos);

			if (missingKeys.Length > 0)
				throw new Exception(
					$"Alguns usuários não foram encontrados. PKs não encontradas: {string.Join(", ", missingKeys)}");

			return dtos.Select(dto => dto.ToDomain()).ToArray();
		}

		public async Task<IEnumerable<User>> ListByChatIdAsync(string chatId)
		{
			throw new NotImplementedException();
		}

		public async Task SafePutAsync(User user)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			await _userContext.SafePutAsync(user.ToDTO());
		}

		public async Task<User> UpdateAsync(User user)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			var dto = await _userContext.UpdateDynamicAsync(user.ToDTO());
			return dto.ToDomain();
		}

		public async Task DeleteAsync(string pk)
		{
			ArgumentException.ThrowIfNullOrEmpty(pk, nameof(pk));

			await _userContext.DeleteAsync(pk, _userSk);
		}

		private static string[] MissingKeys(string[] pks, IEnumerable<UserDTO> dtos)
		{
			return pks.Except(dtos.Where(x => x is not null).Select(x => x.PK)).ToArray();
		}
	}
}
