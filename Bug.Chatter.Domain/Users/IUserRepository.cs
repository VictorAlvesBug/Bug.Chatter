namespace Bug.Chatter.Domain.Users
{
	public interface IUserRepository
	{
		public Task<User?> GetAsync(string pk);

		public Task<User[]> BatchGetAsync(string[] pks);

		//public Task<User[]> ListByChatIdAsync(string chatId);

		public Task SafePutAsync(User user);

		public Task<User> UpdateAsync(User user);

		public Task DeleteAsync(string pk);
	}
}
