using Bug.Chatter.Domain.Models;
using Bug.Chatter.Domain.SeedWork.Exceptions;
using Bug.Chatter.Domain.SeedWork.StringBuilders;
using System.Text.RegularExpressions;

namespace Bug.Chatter.Domain.Users
{
	public class User
	{
		private readonly string _pk;
		private readonly string _sk;
		private readonly string _id;
		private string _name;
		private string _phoneNumber;

		private User(
			string name,
			string phoneNumber)
			: this(
				Database.GenerateUserId(), 
				name,
				phoneNumber) { }

		private User(
			string id,
			string name,
			string phoneNumber)
		{
			_id = id;
			_pk = Database.GetUserPk(id);
			_sk = Database.UserSk;
			_name = name;
			_phoneNumber = phoneNumber;
		}

		public static User CreateNew(string name, string phoneNumber)
		{
			var user = new User(
				name,
				phoneNumber);

			user.CreationValidations();

			return user;
		}

		private void CreationValidations()
		{
			if (string.IsNullOrEmpty(_id))
				throw new BusinessLogicException(string.Format(ErrorReason.IdRequired, nameof(_id)));

			if (string.IsNullOrEmpty(_name))
				throw new BusinessLogicException(string.Format(ErrorReason.NameRequired, nameof(_name)));

			if (string.IsNullOrEmpty(_phoneNumber))
				throw new BusinessLogicException(string.Format(ErrorReason.PhoneNumberRequired, nameof(_phoneNumber)));

			const string pattern = @"^\+\d{1,3} \(\d{1,3}\) \d{4,5}-\d{4}$";

			if (!Regex.IsMatch(_phoneNumber, pattern))
				throw new BusinessLogicException(string.Format(ErrorReason.PhoneNumberInvalid, nameof(_phoneNumber)));
		}

		public static User FromDTO(UserDTO dto)
		{
			return new User(
				id: dto.Id,
				name: dto.Name,
				phoneNumber: dto.PhoneNumber);
		}

		public UserDTO ToDTO()
		{
			return new UserDTO(
				_pk,
				_sk,
				_id,
				_name,
				_phoneNumber);
		}

		public UserModel ToModel()
		{
			return new UserModel(
				_id,
				_name,
				_phoneNumber);
		}
	}
}
