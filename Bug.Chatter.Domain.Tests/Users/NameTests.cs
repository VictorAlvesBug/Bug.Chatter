using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.ValueObjects;
using System.Net.Http.Headers;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class NameTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateName()
		{
			// Arrange
			const string name = "Victor Bugueno";

			// Act
			var actualName = Name.Create(name);

			// Assert
			Assert.That(actualName.Value, Is.EqualTo(name));
			Assert.That(actualName.ToString(), Is.EqualTo(name));
		}

		[Test]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void Create_ShouldThrowsNameIsRequired(string? name)
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => Name.Create(name));
		}
		#endregion
	}
}
