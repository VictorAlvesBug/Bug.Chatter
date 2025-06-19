using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug.Chatter.DataAccess.Repositories.SeedWork.Extensions
{
	public static class DocumentExtensions
	{
		public static T ConvertTo<T>(this Document obj)
			=> obj.ToJson().DeserializeJson<T>();
		
		public static IEnumerable<T> ConvertTo<T>(this IEnumerable<Document> list)
			=> list.Select(obj => obj.ToJson().DeserializeJson<T>());

		public static Document ToDocument<T>(this T t, bool nullValueHandling = false)
			=> Document.FromJson(t.ToJson(nullValueHandling: nullValueHandling));
	}
}
