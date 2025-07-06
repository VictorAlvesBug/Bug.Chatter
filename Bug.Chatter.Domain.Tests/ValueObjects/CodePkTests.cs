using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Tests.ValueObjects
{
	public partial class CodePkTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateCodePk()
		{
			// Arrange
			var numericCode = NumericCode.Generate();

			// Act
			var actualCodePk = CodePk.Create(numericCode);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(actualCodePk.Value, Is.EqualTo($"code-{numericCode.Value}"));
				Assert.That(actualCodePk.ToString(), Is.EqualTo($"code-{numericCode.Value}"));
			});
		}

		[Test]
		public void Create_ShouldThrowsCodePkIsRequired()
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => CodePk.Create(null));
		}
		#endregion
	}
}
