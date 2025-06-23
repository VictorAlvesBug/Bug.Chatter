using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;
using System.Globalization;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Extensions
{
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Converte data e hora local (Brasil) para UTC, para armazenar no banco de dados
		/// </summary>
		/// <param name="brazilianDateTime">Data e hora local (Brasil)</param>
		/// <returns>Data e hora UTC em DateTime</returns>
		public static DateTime ToUtcDateTime(this DateTime brazilianDateTime)
		{
			var utcDateTime = brazilianDateTime.AddHours(+3);
			return DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
		}

		/// <summary>
		/// Converte data e hora local (Brasil) para UTC, para armazenar no banco de dados
		/// </summary>
		/// <param name="strBrazilianDateTime">Data e hora local (Brasil) em string, no formato: dd/MM/yyyy - HH:mm:ss</param>
		/// <returns>Data e hora UTC em DateTime</returns>
		public static DateTime ToUtcDateTime(this string strBrazilianDateTime)
		{
			var brazilianDateTime = DateTime.ParseExact(
				strBrazilianDateTime,
				DatabaseSettings.FrontendDateTimeFormat,
				CultureInfo.InvariantCulture);

			return ToUtcDateTime(brazilianDateTime);
		}

		/// <summary>
		/// Converte data e hora local (Brasil) para UTC, para armazenar no banco de dados
		/// </summary>
		/// <param name="brazilianDateTime">Data e hora local (Brasil)</param>
		/// <returns>Data e hora UTC em string, no formato: yyyy-MM-ddTHH:mm:ss</returns>
		public static string ToUtcStringDateTime(this DateTime brazilianDateTime)
		{
			var databaseDateTime = ToUtcDateTime(brazilianDateTime);
			return databaseDateTime.ToString(DatabaseSettings.DatabaseDateTimeFormat);
		}

		/// <summary>
		/// Converte data e hora local (Brasil) para UTC, para armazenar no banco de dados
		/// </summary>
		/// <param name="strBrazilianDateTime">Data e hora local (Brasil) em string, no formato: dd/MM/yyyy - HH:mm:ss</param>
		/// <returns>Data e hora UTC em DateTime</returns>
		public static string ToUtcStringDateTime(this string strBrazilianDateTime)
		{
			var brazilianDateTime = DateTime.ParseExact(
				strBrazilianDateTime,
				DatabaseSettings.FrontendDateTimeFormat,
				CultureInfo.InvariantCulture);

			return ToUtcStringDateTime(brazilianDateTime);
		}

		/// <summary>
		/// Converte data e hora UTC para local (Brasil), para exibir no front-end
		/// </summary>
		/// <param name="utcDateTime">Data e hora UTC</param>
		/// <returns>Data e hora local (Brasil) em DateTime</returns>
		public static DateTime ToBrazilianDateTime(this DateTime utcDateTime)
		{
			var brazilianDateTime = utcDateTime.AddHours(-3);
			return DateTime.SpecifyKind(brazilianDateTime, DateTimeKind.Local);
		}

		/// <summary>
		/// Converte data e hora UTC para local (Brasil), para exibir no front-end
		/// </summary>
		/// <param name="strUtcDateTime">Data e hora UTC em string, no formato: yyyy-MM-ddTHH:mm:ss</param>
		/// <returns>Data e hora local (Brasil) em DateTime</returns>
		public static DateTime ToBrazilianDateTime(this string strUtcDateTime)
		{
			var utcDateTime = DateTime.ParseExact(
				strUtcDateTime,
				DatabaseSettings.DatabaseDateTimeFormat,
				CultureInfo.InvariantCulture);

			return ToBrazilianDateTime(utcDateTime);
		}

		/// <summary>
		/// Converte data e hora UTC para local (Brasil), para exibir no front-end
		/// </summary>
		/// <param name="utcDateTime">Data e hora UTC</param>
		/// <returns>Data e hora local (Brasil) em string, no formato: dd/MM/yyyy - HH:mm:ss</returns>
		public static string ToBrazilianStringDateTime(this DateTime utcDateTime)
		{
			var frontendDateTime = ToBrazilianDateTime(utcDateTime);
			return frontendDateTime.ToString(DatabaseSettings.FrontendDateTimeFormat);
		}

		/// <summary>
		/// Converte data e hora UTC para local (Brasil), para exibir no front-end
		/// </summary>
		/// <param name="strUtcDateTime">Data e hora UTC em string, no formato: yyyy-MM-ddTHH:mm:ss</param>
		/// <returns>Data e hora local (Brasil) em DateTime</returns>
		public static string ToBrazilianStringDateTime(this string strUtcDateTime)
		{
			var utcDateTime = DateTime.ParseExact(
				strUtcDateTime,
				DatabaseSettings.DatabaseDateTimeFormat,
				CultureInfo.InvariantCulture);

			return ToBrazilianStringDateTime(utcDateTime);
		}
	}
}
