using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Bug.Chatter.Infrastructure.SeedWork.Extensions;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb
{
	public class DynamoUtil : Dictionary<string, DynamoFilter>
	{
		private readonly Dictionary<string, string> _expressionAttributeNames = [];

		private readonly Dictionary<string, DynamoDBEntry> _expressionAttributeValues = [];

		private readonly List<string> _expressionStatement = [];

		public DynamoUtil(IDictionary<string, DynamoFilter> filters = null)
		{
			if (filters != null)
				foreach (var filter in filters)
					base.Add(filter.Key, filter.Value);
		}

		public void Add(string key,
			object value,
			StatementFunction function = StatementFunction.SimpleComparison,
			string comparator = "=",
			string comparisonType = "S",
			bool not = false)
		{
			if (value == null) return;
			if (value is not DynamoFilter vlr)
				vlr = new DynamoFilter(value, function, comparator, comparisonType, not);
			base.Add(key, vlr);
		}

		public Expression? ToExpression()
		{
			foreach (var (key, filter) in this)
			{
				if (IsNullOrWhiteSpace(filter.Value))
					continue;

				var lkey = key.ToLower();

				switch (filter.Value)
				{
					case bool v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case int v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case string v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case long v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case double v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case decimal v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case short v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case char v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case DateTime v:
						SetExpressionAttributeValues(lkey, v);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case Enum v:
						SetExpressionAttributeValues(lkey, v?.GetHashCode() ?? 0);
						_expressionStatement.Add(GetExpression(lkey, filter.Function, filter.Comparator, filter.ComparisonType, filter.Prefix));
						break;
					case IEnumerable<int> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					case IEnumerable<string> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					case IEnumerable<long> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					case IEnumerable<double> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					case IEnumerable<decimal> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					case IEnumerable<short> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					case IEnumerable<DateTime> v:
						AddEnumValueAndStatement(v, lkey, filter.Function);
						break;
					default:
						throw new NotImplementedException($"Tipo {filter.Value.GetType()} não implementado na classe {nameof(DynamoUtil)}");
				}

				_expressionAttributeNames.Add($"#{lkey}", key);
			}

			return _expressionStatement.Count == 0
				? null
				: new Expression
				{
					ExpressionAttributeNames = _expressionAttributeNames,
					ExpressionAttributeValues = _expressionAttributeValues,
					ExpressionStatement = string.Join(" AND ", _expressionStatement)
				};
		}

		private void SetExpressionAttributeValues<T>(string key, T value)
		{
			_expressionAttributeValues[$":{key}"] = DynamoDBEntryConversion.V2.ConvertToEntry(value);
		}

		private static string GetExpression(
			string key,
			StatementFunction function = StatementFunction.SimpleComparison,
			string comparator = "=",
			string comparisonType = "S",
			string prefix = ""
		) => function switch
		{
			_ => $"#{key} {comparator} :{key}",
		};

		private void AddEnumValueAndStatement<T>(
			IEnumerable<T> list,
			string key,
			StatementFunction function = StatementFunction.SimpleComparison)
		{
			var ls = new List<string>();
			var count = 0;
			foreach (var s in list)
			{
				var ks = $":{key}{count}";

				if (IsNullOrWhiteSpace(s))
					continue;

				_expressionAttributeValues[ks] = DynamoDBEntryConversion.V2.ConvertToEntry(s);

				ls.Add(ks);
				count++;
			}

			if (ls.Count > 0)
			{
				var statement = GetInClause(
					key,
					list: ls,
					isNotIn: function == StatementFunction.NotIn);
				_expressionStatement.Add(statement);
			}
		}

		public static string GetInClause(string key, IEnumerable<string> list, bool isNotIn = false)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new Exception($"O parâmetro {nameof(key)} deve ser informado");

			if (list is null || !list.Any())
				throw new Exception($"O parâmetro {nameof(list)} deve ser informado");

			var inClause = $"{string.Join(" OR ", list.Split(100).Select(m => $"#{key} IN ({string.Join(",", m)})"))}";

			return isNotIn ? $"NOT ({inClause})" : $"({inClause})";
		}

		public static bool IsNullOrWhiteSpace<T>(T value)
			=> value switch
			{
				int _ => false,
				null => true,
				string s => string.IsNullOrWhiteSpace(s),
				DateTime d => d == DateTime.MinValue,
				_ => false
			};
	}

	public class DynamoFilter(
		object value,
		StatementFunction function = StatementFunction.SimpleComparison,
		string comparator = "=",
		string comparisonType = "S",
		bool not = false
		)
	{
		public object Value { get; } = value;
		public StatementFunction Function { get; } = function;
		public string Comparator { get; } = comparator;
		public string ComparisonType { get; } = comparisonType;

		public bool Not { get; } = not;

		public string Prefix => Not ? " NOT " : string.Empty;
	}

	public enum StatementFunction
	{
		SimpleComparison,
		//BeginsWith,
		//Contains,
		//Between,
		NotIn
	}
}
