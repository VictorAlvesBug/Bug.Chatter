using Bug.Chatter.Domain.Errors;
using Bug.Chatter.Domain.ValueObjects;
using System;
using System.Net.Http.Headers;

namespace Bug.Chatter.Domain.Tests.ValueObjects
{
	public partial class NumericCodeTests
	{
		#region Create
		[Test]
		public void Create_ShouldCreateNumericCode()
		{
			// Arrange
			string value = new Random().Next(999_999).ToString();

			// Act
			var actualNumericCode = NumericCode.Create(value);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(actualNumericCode.Value, Is.EqualTo(value));
				Assert.That(actualNumericCode.ToString(), Is.EqualTo(value));
			});
		}

		[Test]
		public void Create_ShouldThrowsNumericCodeIsRequired()
		{
			// Act & Assert
			Assert.Throws<DomainException>(() => NumericCode.Create(null));
		}

		[Test]
		public void Create_ShouldThrowsNumericCodeIsInvalid()
		{
			// Arrange
			string value = "invalid code";

			// Act & Assert
			Assert.Throws<DomainException>(() => NumericCode.Create(value));
		}
		#endregion

		#region Generate
		[Test]
		public void Generate_ShouldGenerateNumericCode()
		{
			// Arrange
			const int length = 6;
			const int minValue = 000_000;
			const int maxValue = 999_999;

			// Act
			var numericCode = NumericCode.Generate();

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(numericCode.Value.Length, Is.EqualTo(length));
				Assert.That(int.Parse(numericCode.Value), Is.InRange(minValue, maxValue));

				Assert.That(numericCode.ToString().Length, Is.EqualTo(length));
				Assert.That(int.Parse(numericCode.ToString()), Is.InRange(minValue, maxValue));
			});
		}
		#endregion
	}
}
