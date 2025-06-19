using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug.Chatter.DataAccess.Repositories.SeedWork.Extensions
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

			if(nullValueHandling)
				settings.NullValueHandling = NullValueHandling.Ignore;

			return JsonConvert.SerializeObject(obj, settings);
		}
	}
}
