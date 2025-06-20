using Bug.Chatter.Application.Common;
using Bug.Chatter.Application.Users;
using Bug.Chatter.Domain.SeedWork.ValueObjects;
using Bug.Chatter.Domain.Users;

namespace Bug.Chatter.Application.CreateUser
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
