using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.Users.ValueObjects;

namespace Bug.Chatter.Domain.Tests.User.ValueObjects
{
	public partial class VerificationCodeTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateVerificationCode()
		{
			// Arrange
			string value = new Random().Next(999_999).ToString("000000");

			// Act
			var actualVerificationCode = VerificationCode.Create(value);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(actualVerificationCode.Value, Is.EqualTo(value));
				Assert.That(actualVerificationCode.ToString(), Is.EqualTo(value));
			});
		}

		[Test]
		public void Create_ShouldThrowsVerificationCodeIsRequired()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => VerificationCode.Create(null!));
		}

		[Test]
		public void Create_ShouldThrowsVerificationCodeIsInvalid()
		{
			// Arrange
			string value = "invalid code";

			// Act & Assert
			Assert.Throws<ArgumentException>(() => VerificationCode.Create(value));
		}
		#endregion

		#region Generate
		[Test]
		public void Generate_ShouldGenerateVerificationCode()
		{
			// Arrange
			const int length = 6;
			const int minValue = 000_000;
			const int maxValue = 999_999;

			// Act
			var verificationCode = VerificationCode.Generate();

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(verificationCode.Value.Length, Is.EqualTo(length));
				Assert.That(int.Parse(verificationCode.Value), Is.InRange(minValue, maxValue));

				Assert.That(verificationCode.ToString().Length, Is.EqualTo(length));
				Assert.That(int.Parse(verificationCode.ToString()), Is.InRange(minValue, maxValue));
			});
		}
		#endregion
	}
}
