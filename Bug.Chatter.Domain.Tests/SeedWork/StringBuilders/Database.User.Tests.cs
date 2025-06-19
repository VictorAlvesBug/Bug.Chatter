using Bug.Chatter.Domain.SeedWork.StringBuilders;

namespace Bug.Chatter.Domain.Tests.SeedWork.StringBuilders
{
	public partial class DatabaseTests
	{
		#region GetUserPk
		[Test]
		public void GetUserPk_ShouldReturnUserPkTest()
		{
			// Arrange
			const string id = "e101ed31-6ccf-45e0-91c9-a014b5515871";

			// Act
			var actual = Database.GetUserPk(id);

			// Assert
			Assert.That(actual, Is.EqualTo($"user-{id}"));
		}
		#endregion

		#region GetUserPk
		[Test]
		public void GetUserSk_ShouldReturnUserSkTest()
		{
			// Arrange
			const string expected = "user-mainSchema-v0";

			// Act
			var actual = Database.UserSk;

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}
		#endregion
	}
}
