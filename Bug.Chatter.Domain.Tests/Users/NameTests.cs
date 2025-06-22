using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.ValueObjects;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class NameTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateNameTest()
		{
			// Arrange
			const string value = "Victor Bugueno";

			// Act
			var actualName = Name.Create(value);

			// Assert
			Assert.That(actualName.Value, Is.EqualTo(value));
			Assert.That(actualName.ToString(), Is.EqualTo(value));
		}

		[Test]
		public void Create_ShouldThrowsNameIsRequiredTest()
		{
			// Arrange
			const string value = null;

			// Act & Assert
			Assert.Throws<DomainException>(() => Name.Create(value));
		}
		#endregion
	}
}
