using System.ComponentModel;

namespace Bug.Chatter.Infrastructure.SeedWork.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int size)
		{
			ArgumentNullException.ThrowIfNull(source, nameof(source));
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(source.Count(), nameof(source));

			var numberOfChunks = (float)source.Count() / size;

			for (var i = 0; i < numberOfChunks; i++)
			{
				yield return source.Skip(i * size).Take(size);
			}
		}
	}
}