namespace Bug.Chatter.Infrastructure.SeedWork.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int size)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

			var numberOfChunks = (float)source.Count() / size;

			for (var i = 0; i < numberOfChunks; i++)
			{
				yield return source.Skip(i * size).Take(size);
			}
		}
	}
}