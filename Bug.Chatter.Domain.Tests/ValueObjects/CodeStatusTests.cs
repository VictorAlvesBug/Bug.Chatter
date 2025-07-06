using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Tests.ValueObjects
{
	public partial class CodeStatusTests
	{
		#region From
		[Test]
		[TestCase("NotSentYet")]
		[TestCase("Sent")]
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
			Assert.Throws<DomainException>(() => CodeStatus.From(value));
		}
		#endregion
	}
}
