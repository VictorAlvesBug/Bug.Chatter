using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Models;
using Bug.Chatter.Domain.SeedWork.ValueObjects;

namespace Bug.Chatter.Domain.Users
{
	public class User
	{
		private readonly UserPk _pk;
		private readonly UserSk _sk;
		private readonly UserId _id;
		private Name _name;
		private PhoneNumber _phoneNumber;

		private User(
			Name name,
			PhoneNumber phoneNumber)
			: this(
				UserId.Generate(), 
				name,
				phoneNumber) { }

		private User(
			UserId id,
			Name name,
			PhoneNumber phoneNumber)
		{
			_id = id;
			_pk = UserPk.Create(_id);
			_sk = UserSk.Create();
			_name = name;
			_phoneNumber = phoneNumber;
		}

		public static User CreateNew(Name name, PhoneNumber phoneNumber)
		{
			//CreationValidations(name, phoneNumber);

			return new User(name, phoneNumber);
		}

		/*private static void CreationValidations(Name name, PhoneNumber phoneNumber)
		{
			// ...
		}*/

		public static User LoadFromPersistence(UserDTO dto)
		{
			if (!Guid.TryParse(dto.Id, out var parsedId))
				throw new DomainException(string.Format(ErrorReason.User.InvalidIdLoaded, nameof(UserId)));

			return new User(
				id: UserId.Create(Guid.Parse(dto.Id)),
				name: Name.Create(dto.Name),
				phoneNumber: PhoneNumber.Create(dto.PhoneNumber));
		}

		public UserDTO ToDTO()
		{
			return new UserDTO(
				_pk.ToString(),
				_sk.Value,
				_id.ToString(),
				_name.ToString(),
				_phoneNumber.ToString());
		}

		public UserModel ToModel()
		{
			return new UserModel(
				_id.ToString(),
				_name.ToString(),
				_phoneNumber.ToString());
		}
	}
}
