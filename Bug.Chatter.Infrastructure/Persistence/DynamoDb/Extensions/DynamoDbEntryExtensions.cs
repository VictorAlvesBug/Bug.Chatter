using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions
{
	internal static class DynamoDbEntryExtensions
	{
		public static AttributeValue AsAttributeValue(this DynamoDBEntry dynamoDbEntry)
		{
			return DynamoDBEntryConversion.V2.ConvertFromEntry<AttributeValue>(dynamoDbEntry);
		}
	}
}
