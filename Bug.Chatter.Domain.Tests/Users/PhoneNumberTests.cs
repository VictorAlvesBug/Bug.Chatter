using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.SeedWork.ValueObjects;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class PhoneNumberTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreatePhoneNumber()
		{
			// Arrange
			const string phoneNumber = "+55 (11) 97562-3736";

			// Act
			var actualPhoneNumber = PhoneNumber.Create(phoneNumber);

			// Assert
			Assert.That(actualPhoneNumber.Value, Is.EqualTo(phoneNumber));
			Assert.That(actualPhoneNumber.ToString(), Is.EqualTo(phoneNumber));
		}

		[Test]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void Create_ShouldThrowsPhoneNumberIsRequired(string? phoneNumber)
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => PhoneNumber.Create(phoneNumber));
		}

		[Test]
		[TestCase("abc")]
		[TestCase("123")]
		[TestCase("5511975623736")]
		public void Create_ShouldThrowsPhoneNumberIsInvalid(string? phoneNumber)
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => PhoneNumber.Create(phoneNumber));
		}
		#endregion
	}
}
