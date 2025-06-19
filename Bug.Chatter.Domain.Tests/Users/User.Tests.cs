using Bug.Chatter.Domain.SeedWork.Exceptions;
using Bug.Chatter.Domain.Users;
using System.Xml.Linq;

namespace Bug.Chatter.Domain.Tests.Users
{
	public partial class UserTests
	{
		#region CreateNew
		[Test]
		public void CreateNew_ShouldReturnNewUserTest()
		{
			// Arrange
			const string name = "Victor Bugueno";
			const string phoneNumber = "+55 (11) 97562-3736";

			// Act
			var actualUser = User.CreateNew(name, phoneNumber);

			// Assert
			Assert.IsNotNull(actualUser);
		}

		[Test]
		public void CreateNew_ShouldThrowsNameIsRequiredTest()
		{
			// Arrange
			const string name = null;
			const string phoneNumber = "+55 (11) 97562-3736";

			// Act & Assert
			var exception = Assert.Throws<BusinessLogicException>(() => User.CreateNew(name, phoneNumber));
			Assert.That(exception.Message, Is.EqualTo(string.Format(ErrorReason.NameRequired, nameof(name))));
		}

		[Test]
		public void CreateNew_ShouldThrowsPhoneNumberIsRequiredTest()
		{
			// Arrange
			const string name = "Victor Bugueno";
			const string phoneNumber = null;

			// Act & Assert
			var exception = Assert.Throws<BusinessLogicException>(() => User.CreateNew(name, phoneNumber));
			Assert.That(exception.Message, Is.EqualTo(string.Format(ErrorReason.PhoneNumberRequired, nameof(phoneNumber))));
		}

		[Test]
		public void CreateNew_ShouldThrowsPhoneNumberIsInvalidTest()
		{
			// Arrange
			const string name = "Victor Bugueno";
			const string phoneNumber = "xpto";

			// Act & Assert
			var exception = Assert.Throws<BusinessLogicException>(() => User.CreateNew(name, phoneNumber));
			Assert.That(exception.Message, Is.EqualTo(string.Format(ErrorReason.PhoneNumberInvalid, nameof(phoneNumber))));
		}
		#endregion
	}
}
