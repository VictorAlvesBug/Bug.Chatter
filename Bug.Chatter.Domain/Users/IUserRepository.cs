namespace Bug.Chatter.Domain.Users
{
	public interface IUserRepository
	{
		public Task<User?> GetAsync(string pk);

		public Task<User[]> BatchGetAsync(string[] pks);

		public Task SafePutAsync(User user);

		public Task UpdateAsync(User user, int expectedVersion);

		public Task DeleteAsync(string pk, int expectedVersion);
	}
}
