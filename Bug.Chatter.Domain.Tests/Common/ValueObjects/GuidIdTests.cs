using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.Errors;

namespace Bug.Chatter.Domain.Tests.Common.ValueObjects
{
	public partial class GuidIdTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateGuidId()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			// Act
			var actualGuidId = GuidId.Create(guid);

			Assert.Multiple(() =>
			{
				// Assert
				Assert.That(actualGuidId.Value, Is.EqualTo(guid.ToString()));
				Assert.That(actualGuidId.ToString(), Is.EqualTo(guid.ToString()));
			});
		}

		[Test]
		public void Create_ShouldThrowsGuidIdIsRequired()
		{
			// Arrange
			Guid guid = Guid.Empty;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => GuidId.Create(guid));
		}
		#endregion

		#region Generate
		[Test]
		public void Generate_ShouldGenerateGuidId()
		{
			// Act
			var guidId = GuidId.Generate();

			// Assert
			Assert.DoesNotThrow(() =>
			{
				var guidFromValue = new Guid(guidId.Value);
				var guidFromToString = new Guid(guidId.ToString());

				Assert.Multiple(() =>
				{
					Assert.That(guidFromValue, Is.Not.EqualTo(Guid.Empty));
					Assert.That(guidFromToString, Is.Not.EqualTo(Guid.Empty));
				});
			});
		}
		#endregion
	}
}
