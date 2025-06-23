using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class UserPkTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateUserPk()
		{
			// Arrange
			var userId = UserId.Generate();

			// Act
			var actualUserPk = UserPk.Create(userId);

			// Assert
			Assert.That(actualUserPk.Value, Is.EqualTo($"user-{userId.Value}"));
			Assert.That(actualUserPk.ToString(), Is.EqualTo($"user-{userId.Value}"));
		}

		[Test]
		public void Create_ShouldThrowsUserPkIsRequired()
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => UserPk.Create(null));
		}
		#endregion
	}
}
