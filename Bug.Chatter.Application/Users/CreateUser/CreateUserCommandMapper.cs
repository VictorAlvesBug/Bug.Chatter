using Bug.Chatter.Application.Common;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Application.Users.CreateUser
{
	internal class CreateUserCommandMapper : ICommandMapper<CreateUserCommand, User>
	{
		public User Map(CreateUserCommand input)
		{
			var name = Name.Create(input.Name);
			var phoneNumber = PhoneNumber.Create(input.PhoneNumber);
			return User.CreateNew(name, phoneNumber);
		}
	}
}
