using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Aggregates.Codes
{
	public interface ICodeRepository
	{
		public Task<Code?> GetAsync(CodePk pk);
		public Task SafePutAsync(Code code);
		public Task<IEnumerable<Code>> ListByPhoneNumberAsync(PhoneNumber phoneNumber);
	}
}
