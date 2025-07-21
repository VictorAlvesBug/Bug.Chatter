using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;
using System.Text;

namespace Bug.Chatter.Domain.Tests.User.ValueObjects
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
			Assert.Throws<ArgumentNullException>(() => Name.Create(name!));
		}

		[Test]
		public void Create_ShouldThrowsNameIsTooLarge()
		{
			// Arrange
			const int maxNameLength = 50;
			var sbName = new StringBuilder();

			for (int i = 0; i < maxNameLength + 1; i++)
			{
				sbName.Append('#');
			}

			// Act & Assert
			Assert.Throws<ArgumentException>(() => Name.Create(sbName.ToString()));
		}
		#endregion
	}
}
