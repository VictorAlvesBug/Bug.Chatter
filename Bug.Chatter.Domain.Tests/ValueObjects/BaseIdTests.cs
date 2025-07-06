using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;

namespace Bug.Chatter.Domain.Tests.ValueObjects
{
	public partial class NumericCodeTests
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

		#region Generate
		[Test]
		public void Generate_ShouldGenerateBaseId()
		{
			// Act
			var baseId = BaseId.Generate();

			// Assert
			Assert.DoesNotThrow(() => {
				var guidFromValue = new Guid(baseId.Value);
				var guidFromToString = new Guid(baseId.ToString());

				Assert.Multiple(() =>
				{
					Assert.That(guidFromValue == Guid.Empty, Is.False);
					Assert.That(guidFromToString == Guid.Empty, Is.False);
				});
			});
		}
		#endregion
	}
}
