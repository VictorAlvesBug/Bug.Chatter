using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Aggregates.Users
{
	public interface IUserRepository
	{
		public Task<User?> GetAsync(UserPk pk);
		public Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber);

		public Task<User[]> BatchGetAsync(UserPk[] pks);

		public Task SafePutAsync(User user);

		public Task UpdateAsync(User user, int expectedVersion);

		public Task DeleteAsync(UserPk pk, int expectedVersion);
	}
}
