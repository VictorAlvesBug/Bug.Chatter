using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;
using System.Net.Http.Headers;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class UserIdTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateUserId()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			// Act
			var actualUserId = UserId.Create(guid);

			// Assert
			Assert.That(actualUserId.Value, Is.EqualTo(guid.ToString()));
			Assert.That(actualUserId.ToString(), Is.EqualTo(guid.ToString()));
		}

		[Test]
		public void Create_ShouldThrowsUserIdIsRequired()
		{
			// Arrange
			Guid guid = Guid.Empty;

			// Act & Assert
			Assert.Throws<DomainException>(() => UserId.Create(guid));
		}
		#endregion
	}
}
