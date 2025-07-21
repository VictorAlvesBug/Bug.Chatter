using Bug.Chatter.Application.Common;
using Bug.Chatter.Domain.Users.Entities;

namespace Bug.Chatter.Application.Users.InitializeUser
{
	internal class InitializeUserCommandMapper : ICommandMapper<InitializeUserCommand, User>
	{
		public User Map(InitializeUserCommand input)
		{
			return User.CreateNew(input.Name, input.PhoneNumber);
		}
	}
}
