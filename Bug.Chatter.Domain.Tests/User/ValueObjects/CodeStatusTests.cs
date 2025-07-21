using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Tests.User.ValueObjects
{
	public partial class CodeStatusTests
	{
		#region From
		[Test]
		[TestCase(nameof(CodeStatus.NotSentYet))]
		[TestCase(nameof(CodeStatus.Sent))]
		public void From_ShouldCreateCodeStatus(string value)
		{
			// Act
			var actualCodeStatus = CodeStatus.From(value);

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
			Assert.Throws<ArgumentException>(() => CodeStatus.From(value!));
		}
		#endregion
	}
}
