using Bug.Chatter.Domain.SeedWork.StringBuilders;

namespace Bug.Chatter.Domain.Tests.SeedWork.StringBuilders;

public partial class DatabaseTests
{
	[SetUp]
	public void Setup()
	{
	}

	#region Hyphenize
	[Test]
	public void Hyphenize_ShouldReturnKeyWithThreePartsTest()
	{
		// Arrange
		const string expected = @"part1-part2-part3";

		// Act
		var actual = Database.Hyphenize("part1", "part2", "part3");

		// Assert
		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Hyphenize_ShouldThrowsTest()
	{
		// Act & Assert
		Assert.Throws<ArgumentException>(() => Database.Hyphenize());
	}
	#endregion
}