using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;
using System.Net.Http.Headers;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class BaseIdTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateBaseId()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			// Act
			var actualBaseId = BaseId.Create(guid);

			// Assert
			Assert.That(actualBaseId.Value, Is.EqualTo(guid.ToString()));
			Assert.That(actualBaseId.ToString(), Is.EqualTo(guid.ToString()));
		}

		[Test]
		public void Create_ShouldThrowsBaseIdIsRequired()
		{
			// Arrange
			Guid guid = Guid.Empty;

			// Act & Assert
			Assert.Throws<DomainException>(() => BaseId.Create(guid));
		}
		#endregion
	}
}
