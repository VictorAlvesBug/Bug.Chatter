using Bug.Chatter.Domain.SeedWork.StringBuilders;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.DataAccess.Repositories.Users
{
	internal class UserRepository : IUserRepository
	{
		private readonly UserContext _userContext;

		public UserRepository(UserContext userContext)
		{
			_userContext = userContext;
		}

		public async Task<User?> GetAsync(string pk)
		{
			ArgumentException.ThrowIfNullOrEmpty(pk, nameof(pk));

			var dto = await _userContext.GetAsync(pk, Database.UserSk);
			return dto is not null ? User.LoadFromPersistence(dto) : null;
		}

		public async Task<User[]> BatchGetAsync(string[] pks)
		{
			ArgumentNullException.ThrowIfNull(pks, nameof(pks));
			
			var dtos = await _userContext.BatchGetAsync(pks.Select(pk => ((dynamic) pk, (dynamic) Database.UserSk)));

			var missingKeys = MissingKeys(pks, dtos);

			if (missingKeys.Length > 0)
				throw new Exception(
					$"Alguns usuários não foram encontrados. PKs não encontradas: {string.Join(", ", missingKeys)}");

			return dtos.Select(User.LoadFromPersistence).ToArray();
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
			return User.LoadFromPersistence(dto);
		}

		public async Task DeleteAsync(string pk)
		{
			ArgumentException.ThrowIfNullOrEmpty(pk, nameof(pk));

			await _userContext.DeleteAsync(pk, Database.UserSk);
		}

		private static string[] MissingKeys(string[] pks, IEnumerable<UserDTO> dtos)
		{
			return pks.Except(dtos.Where(x => x is not null).Select(x => x.PK)).ToArray();
		}
	}
}
