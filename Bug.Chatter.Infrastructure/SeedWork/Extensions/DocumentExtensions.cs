using Amazon.DynamoDBv2.DocumentModel;

namespace Bug.Chatter.Infrastructure.SeedWork.Extensions
{
	public static class DocumentExtensions
	{
		public static T? ConvertTo<T>(this Document obj)
		{
			ArgumentNullException.ThrowIfNull(obj, nameof(obj));

			return obj.ToJson().DeserializeJson<T>();
		}

		public static IEnumerable<T> ConvertTo<T>(this IEnumerable<Document> list)
			=> list.Select(obj => obj.ToJson().DeserializeJson<T>())
				.Where(obj => obj is not null).Select(obj => obj!);

		public static Document ToDocument<T>(this T t, bool nullValueHandling = false)
			=> Document.FromJson(t.ToJson(nullValueHandling: nullValueHandling));
	}
}
