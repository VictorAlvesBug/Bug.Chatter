using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug.Chatter.DataAccess.Repositories.SeedWork.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int size)
		{
			var numberOfChunks = (float)source.Count() / size;

			for (var i = 0; i < numberOfChunks; i++)
			{
				yield return source.Skip(i * size).Take(size);
			}
		}
	}
}