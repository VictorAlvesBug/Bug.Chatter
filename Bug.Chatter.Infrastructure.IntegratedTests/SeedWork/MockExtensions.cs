using Moq;
using System.Linq.Expressions;

namespace Bug.Chatter.Infrastructure.IntegratedTests.SeedWork
{
	public static class MockExtensions
	{
		public static IInvocation? GetLastInvocationOf<T>(this Mock<T> mock, string methodName)
			where T : class => mock.Invocations.LastOrDefault(i => i.Method.Name == methodName);
	}
}
