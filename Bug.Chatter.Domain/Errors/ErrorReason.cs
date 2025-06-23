namespace Bug.Chatter.Domain.Errors
{
	public static class ErrorReason
	{
		public static class EventStore
		{
			public const string AggregatePkRequired = "{0}: PK do recurso agregado deve ser fornecida";
			public const string AggregateSkRequired = "{0}: SK do recurso agregado deve ser fornecida";
			public const string VersionInvalid = "{0}: Versão do recurso agregado deve ser um número inteiro, positivo e diferente de zero";
			public const string EventTypeInvalid = "{0}: Tipo de evento inválido";
			public const string TimestampRequired = "{0}: Timestamp deve ser fornecido";
			public const string EventDataInvalid = "{0}: Os dados do evento são inválidos";
			public const string AttributeDoesNotExists = "{0}: Atributo informado não existe no recurso agregado";
			public const string WrongAttributeType = "{0}: Atributo informado com tipo errado. Esperado: {1}; Recebido: {2}";
		}

		public static class User
		{
			public const string IdRequired = "{0}: Id do usuário deve ser fornecido";
			public const string InvalidIdLoaded = "{0}: Id inválido ao carregar usuário";
			public const string NameRequired = "{0}: Nome do usuário deve ser fornecido";
			public const string PhoneNumberRequired = "{0}: Número de celular do usuário deve ser fornecido";
			public const string PhoneNumberInvalid = "{0}: Informe um número de telefone válido. Ex: +55 (11) 91234-5678";
		}
	}
}
