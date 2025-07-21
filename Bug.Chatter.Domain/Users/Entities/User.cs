using Bug.Chatter.Domain.Common;
using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Users.Entities
{
	public class User : Entity<GuidId>
	{
		public Name Name { get; private set; }
		public PhoneNumber PhoneNumber { get; private set; }
		public UserStatus Status { get; private set; }
		public int Version { get; private set; }
		public DateTime CreatedAt { get; protected init; }


		private readonly List<UserCode> _codes = [];
		public IReadOnlyCollection<UserCode> Codes => _codes.AsReadOnly();

		private User(
			Name name,
			PhoneNumber phoneNumber)
			: this(
				id: GuidId.Generate(),
				name,
				phoneNumber,
				status: UserStatus.Draft,
				version: 1,
				createdAt: DateTime.UtcNow,
				codes: [])
		{ }

		private User(
			GuidId id,
			Name name,
			PhoneNumber phoneNumber,
			UserStatus status,
			int version,
			DateTime createdAt,
			List<UserCode> codes)
			: base(id)
		{
			Name = name;
			PhoneNumber = phoneNumber;
			Status = status;
			Version = version;
			CreatedAt = createdAt;
			_codes = codes;
		}

		public void UpdateName(Name name)
		{
			Name = name;
		}

		public void UpdateStatus(UserStatus status)
		{
			Status = status;
		}

		public void Register()
		{
			if (Status != UserStatus.Draft)
				throw new InvalidOperationException(string.Format(ErrorReason.User.OnlyDraftUsersCanBeRegistered, nameof(Status), Status));

			Status = UserStatus.Registered;
		}

		public void AddVerificationCode()
		{
			_codes.Add(new UserCode());
		}

		public bool ValidateCode(VerificationCode verificationCode)
		{
			var code = _codes.Find(c => c.VerificationCode == verificationCode);

			return code?.IsValid() ?? false;
		}

		public void IncrementVersion()
		{
			Version++;
		}

		public static User Rehydrate(
			GuidId id,
			Name name,
			PhoneNumber phoneNumber,
			UserStatus status,
			int version,
			DateTime createdAt,
			List<UserCode> codes)
		{
			return new User(id, name, phoneNumber, status, version, createdAt, codes);
		}

		public static User CreateNew(Name name, PhoneNumber phoneNumber)
		{
			return new User(name, phoneNumber);
		}

		public static bool StatusAllowInitializeUser(UserStatus status)
		{
			var allowedStatuses = new List<UserStatus>
			{
				UserStatus.Draft,
				UserStatus.Deleted
			};

			return allowedStatuses.Contains(status);
		}
	}
}
