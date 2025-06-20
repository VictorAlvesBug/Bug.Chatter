using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bug.Chatter.Infrastructure.SeedWork.Extensions
{
	public static class SerializingExtensions
	{
		private static readonly JsonSerializerSettings DefaultSettings = new()
		{
			DateFormatHandling = DateFormatHandling.IsoDateFormat,
			DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
			Culture = System.Globalization.CultureInfo.CurrentCulture,
			DateFormatString = "yyyy-MM-dd",
			DateParseHandling = DateParseHandling.None
		};

		public static T? DeserializeJson<T>(this string json)
		{
			ArgumentNullException.ThrowIfNull(json, nameof(json));

			return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
		}
	}
}
