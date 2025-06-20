using Newtonsoft.Json;

namespace Bug.Chatter.Infrastructure.SeedWork.Extensions
{
	public static class ObjectExtension
	{
		public static string ToJson(
			this object obj,
			bool nullValueHandling = false)
		{
			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.None
			};

			if (nullValueHandling)
				settings.NullValueHandling = NullValueHandling.Ignore;

			return JsonConvert.SerializeObject(obj, settings);
		}
	}
}
