using Amazon.DynamoDBv2.DocumentModel;

namespace Bug.Chatter.Infrastructure.SeedWork.Extensions
{
	public static class DocumentExtensions
	{
		public static T? ConvertTo<T>(this Document obj)
		{
			if (obj is null) throw new ArgumentNullException(nameof(obj));

			return obj.ToJson().DeserializeJson<T>();
		}

		public static IEnumerable<T?> ConvertTo<T>(this IEnumerable<Document> list)
			=> list.Select(obj => obj.ToJson().DeserializeJson<T>());

		public static Document ToDocument<T>(this T t, bool nullValueHandling = false)
			=> Document.FromJson(t.ToJson(nullValueHandling: nullValueHandling));
	}
}
