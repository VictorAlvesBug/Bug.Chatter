﻿using Bug.Chatter.Domain.Aggregates.Users;
using Bug.Chatter.Domain.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions;
using System.Globalization;
using System.Runtime.Serialization;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public static class UserDbMapperExtensions
	{
		public static User ToDomain(this UserDTO dto)
		{
			return User.Rehydrate(
				id: BaseId.Create(Guid.Parse(dto.Id)),
				name: Name.Create(dto.Name),
				phoneNumber: PhoneNumber.Create(dto.PhoneNumber),
				version: dto.Version,
				createdAt: dto.CreatedAt.ToBrazilianDateTime()
			);
		}

		public static UserDTO ToDTO(this User domain, string userSk)
		{
			return new UserDTO(
				pk: domain.Pk.Value,
				sk: userSk,
				id: domain.Id.Value,
				name: domain.Name.Value,
				phoneNumber: domain.PhoneNumber.Value,
				version: domain.Version,
				createdAt: domain.CreatedAt.ToUtcStringDateTime()
			);
		}

	}
}
