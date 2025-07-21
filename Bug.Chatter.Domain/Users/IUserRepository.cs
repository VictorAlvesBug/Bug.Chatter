using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users.Entities;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Users
{
	public interface IUserRepository
	{
		public Task<User?> GetByUserIdAsync(GuidId id, IUserLoadSpecification spec);
		public Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber, IUserLoadSpecification spec);

		public Task<IEnumerable<User>> ListByIdsAsync(IEnumerable<GuidId> ids, IUserLoadSpecification spec);

		public Task SaveAsync(User user, IUserLoadSpecification spec);

		public Task DeleteAsync(User user);
	}
}
