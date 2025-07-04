﻿using Bug.Chatter.Application.Common;
using Bug.Chatter.Domain.Aggregates.Users;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Application.Aggregates.Users.RegisterUser
{
	internal class RegisterUserCommandMapper : ICommandMapper<RegisterUserCommand, User>
	{
		public User Map(RegisterUserCommand input)
		{
			var name = Name.Create(input.Name);
			var phoneNumber = PhoneNumber.Create(input.PhoneNumber);
			return User.CreateNew(name, phoneNumber);
		}
	}
}
