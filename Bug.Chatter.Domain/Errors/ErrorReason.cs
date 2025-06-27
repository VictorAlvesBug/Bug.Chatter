namespace Bug.Chatter.Domain.Errors
{
	public static class ErrorReason
	{
		public static class BaseId
		{
			public const string IdRequired = "{0}: Id de recurso deve ser fornecido";
			public const string InvalidIdLoaded = "{0}: Id inválido ao carregar recurso";
		}

		public static class User
		{
			public const string NameRequired = "{0}: Nome do usuário deve ser fornecido";
			public const string NameTooLarge = "{0}: Nome do usuário não pode exceder {1} caracteres";
			public const string PhoneNumberRequired = "{0}: Número de celular do usuário deve ser fornecido";
			public const string PhoneNumberInvalid = "{0}: Informe um número de celular válido. Ex: +55 (11) 91234-5678";
			public const string PhoneNumberMustBeUnique = "{0}: Número de celular '{1}' já cadastrado";
		}

		public static class  Message
		{
			public const string ContentRequired = "{0}: Conteúdo da mensagem deve ser fornecido";
			public const string ContentTooLarge = "{0}: Conteúdo da mensagem não pode exceder {1} caracteres";
		}
	}
}
