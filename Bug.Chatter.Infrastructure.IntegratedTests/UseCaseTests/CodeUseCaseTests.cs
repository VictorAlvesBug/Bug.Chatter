using Bug.Chatter.Application.DependencyInjection;
using Bug.Chatter.Infrastructure.DependencyInjection;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Codes;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bug.Chatter.Infrastructure.IntegratedTests.UseCaseTests
{
	[TestFixture]
	public partial class CodeUseCaseTests
	{
		private readonly Mock<ICodeContext> _mockCodeContext;

		private readonly IServiceProvider _scopeProvider;

		public CodeUseCaseTests()
		{
			var services = new ServiceCollection();
			services.AddApplicationServices();
			services.AddInfrastructureServices();

			_mockCodeContext = new Mock<ICodeContext>();
			OverrideWithMockCodeContext(services, _mockCodeContext);

			var _rootProvider = services.BuildServiceProvider(validateScopes: true);
			_scopeProvider = _rootProvider.CreateScope().ServiceProvider;
		}

		[SetUp]
		public void Setup()
		{
			Mock.Get(_mockCodeContext.Object).Invocations.Clear();
		}

		private ServiceCollection OverrideWithMockCodeContext(ServiceCollection services, Mock<ICodeContext> mockCodeContext)
		{
			CodeDTO[] mockedCodes = [
				new(
					pk: "code-123456",
					sk: "code-mainSchema-v0",
					numericCode: "123456",
					phoneNumber: "+55 (11) 97562-3736",
					status: "Sent",
					version: 999,
					createdAt: "2025-06-27T00:00:00",
					expiresAt: "2025-06-27T00:10:00",
					ttl: 1750973400),
				new(
					pk: "code-654321",
					sk: "code-mainSchema-v0",
					numericCode: "654321",
					phoneNumber: "+55 (11) 98237-5687",
					status: "NotSentYet",
					version: 999,
					createdAt: "2025-07-06T00:00:00",
					expiresAt: "2025-07-06T00:10:00",
					ttl: 1751741400)
			];

			mockCodeContext
				.Setup(mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string pk, string sk, List<string> _) =>
					mockedCodes.FirstOrDefault(code => code.PK == pk && code.SK == sk)
				);

			mockCodeContext
				.Setup(mock => mock.ListByIndexKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string indexName, string indexPkValue, string indexSkValue, List<string> _) =>
				{
					var parts = indexName.Split('-');
					var indexPkName = parts[0];
					var indexSkName = parts[1];

					return mockedCodes.Where(code =>
					{
						var codeIndexPkValue = code.GetType().GetProperty(indexPkName)?.GetValue(code)?.ToString();
						var codeIndexSkValue = code.GetType().GetProperty(indexSkName)?.GetValue(code)?.ToString();

						if (indexSkValue is null)
							return codeIndexPkValue == indexPkValue;

						return codeIndexPkValue == indexPkValue && codeIndexSkValue == indexSkValue;
					});
				});

			mockCodeContext
				.Setup(mock => mock.BatchGetAsync(It.IsAny<IEnumerable<(string, string)>>(), It.IsAny<List<string>>()))
				.ReturnsAsync((IEnumerable<(string, string)> keysList, List<string> _) => 
					mockedCodes.Where(code => 
						keysList.Any(keys => keys.Item1 == code.PK && keys.Item2 == code.SK)
					)
				);

			mockCodeContext
				.Setup(mock => mock.ListByPartitionKeyAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
				.ReturnsAsync((string pk, List<string> _) =>
					mockedCodes.Where(code => code.PK == pk)
				);

			mockCodeContext
				.Setup(mock => mock.SafePutAsync(It.IsAny<CodeDTO>()));

			mockCodeContext
				.Setup(mock => mock.UpdateDynamicAsync(It.IsAny<CodeDTO>()));

			mockCodeContext
				.Setup(mock => mock.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()));


			services.AddScoped(_ => mockCodeContext.Object);

			return services;
		}
	}
}