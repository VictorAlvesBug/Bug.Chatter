using Bug.Chatter.Application.Common;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Users.ValidateNew
{
	internal class ValidateNewUserCommandMapper : ICommandMapper<ValidateNewUserCommand, User>
	{
		public User Map(ValidateNewUserCommand input)
		{
			var name = Name.Create(input.Name);
			var phoneNumber = PhoneNumber.Create(input.PhoneNumber);
			return User.CreateNew(name, phoneNumber);
		}
	}
}
