using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Tests.User.ValueObjects
{
	public partial class UserStatusTests
	{
		#region From
		[Test]
		[TestCase(nameof(UserStatus.Draft))]
		[TestCase(nameof(UserStatus.Registered))]
		[TestCase(nameof(UserStatus.Deleted))]
		public void From_ShouldCreateCodeStatus(string value)
		{
			// Act
			var actualCodeStatus = UserStatus.From(value);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(actualCodeStatus.Value, Is.EqualTo(value));
				Assert.That(actualCodeStatus.ToString(), Is.EqualTo(value));
			});
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("invalid status")]
		public void From_ShouldThrowsCodeStatusIsInvalid(string? value)
		{
			// Act & Assert
			Assert.Throws<ArgumentException>(() => UserStatus.From(value!));
		}
		#endregion
	}
}
