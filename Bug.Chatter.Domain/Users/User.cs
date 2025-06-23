using Bug.Chatter.Domain.SeedWork;
using Bug.Chatter.Domain.Users.Events;
using Bug.Chatter.Domain.Users.ValueObjects;
using System.Xml.Linq;
using System;

namespace Bug.Chatter.Domain.Users
{
	public class User : AggregateRoot<UserPk, string>
	{
		public UserId Id { get; private set; }
		public Name Name { get; private set; }
		public PhoneNumber PhoneNumber { get; private set; }

		private User() { }

		/*private User(
			Name name,
			PhoneNumber phoneNumber)
			: this(
				id: UserId.Generate(),
				name,
				phoneNumber,
				version: 1) { }

		private User(
			UserId id,
			Name name,
			PhoneNumber phoneNumber,
			int version)
		{
			Id = id;
			Pk = UserPk.Create(Id);
			Name = name;
			PhoneNumber = phoneNumber;
			Version = version;
		}*/

		public static User Rehydrate(IEnumerable<IDomainEvent> events)
		{
			var user = new User();

			foreach (var @event in events)
				user.ApplyEvent((dynamic)@event);

			return user;
		}

		public static User CreateNew(Name name, PhoneNumber phoneNumber)
		{
			var user = new User();

			var @event = new UserCreated(
				Id: UserId.Generate(),
				Name: name,
				PhoneNumber: phoneNumber,
				Version: 1
			);

			user.ApplyEvent(@event);
			user.DomainEvents.Add(@event);

			return user;
		}

		private void ApplyEvent(UserCreated e)
		{
			Pk = UserPk.Create(e.Id);
			Sk = e.AggregateSk;
			Id = e.Id;
			Name = e.Name;
			PhoneNumber = e.PhoneNumber;
			Version = e.Version;
		}

		private void ApplyEvent(IDomainEvent e)
		{
			throw new InvalidOperationException($"Evento não tratado: {e.GetType().Name}");
		}
	}
}
