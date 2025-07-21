using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public static class KeyFactory
	{
		public const string MainSchemaV0 = "mainSchema-v0";
		public const string UserPrefix = "user";
		public const string CodePrefix = "code";

		public static string UserPk(GuidId id)
		{
			if (id is null || id == Guid.Empty)
				throw new ArgumentNullException(nameof(id));

			return Hyphenize(UserPrefix, id.Value);
		}
		public static string UserSk() => Hyphenize(UserPrefix, MainSchemaV0);

		public static string CodePk(GuidId id) => UserPk(id);

		public static string CodeSk(VerificationCode verificationCode)
		{
			if (verificationCode is null || string.IsNullOrWhiteSpace(verificationCode.Value))
				throw new ArgumentNullException(nameof(verificationCode));

			return Hyphenize(CodePrefix, verificationCode.Value);
		}

		public static string Hyphenize(params string[] parts)
		{
			if (parts is null || parts.Length == 0)
				throw new ArgumentNullException(nameof(parts));

			var validParts = parts.Where(part => !string.IsNullOrWhiteSpace(part));

			return string.Join('-', validParts);
		}
	}
}
