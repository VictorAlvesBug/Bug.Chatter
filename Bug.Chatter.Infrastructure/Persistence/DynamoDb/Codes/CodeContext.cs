using Amazon.DynamoDBv2;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using Microsoft.Extensions.Caching.Memory;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes
{
	internal class CodeContext : GenericDynamoDbRepository<CodeDTO>, ICodeContext
	{
		public CodeContext(
			IAmazonDynamoDB ddbClient,
			IMemoryCache memoryCache)
			: base(ddbClient, DatabaseSettings.ChatterTableName, memoryCache)
		{
			
		}
	}
}
