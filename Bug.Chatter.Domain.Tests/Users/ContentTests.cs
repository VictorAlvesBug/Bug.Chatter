using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;
using System.Text;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class ContentTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateContent()
		{
			// Arrange
			const string content = "Hello World";

			// Act
			var actualContent = Content.Create(content);

			// Assert
			Assert.That(actualContent.Value, Is.EqualTo(content));
			Assert.That(actualContent.ToString(), Is.EqualTo(content));
		}

		[Test]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void Create_ShouldThrowsContentIsRequired(string? content)
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => Content.Create(content));
		}

		[Test]
		public void Create_ShouldThrowsContentIsTooLarge()
		{
			// Arrange
			const int maxContentLength = 500;
			var sbContent = new StringBuilder();

			for (int i = 0; i < maxContentLength+1; i++) {
				sbContent.Append('#');
			}

			// Act & Assert
			Assert.Throws<DomainException>(() => Content.Create(sbContent.ToString()));
		}
		#endregion
	}
}
