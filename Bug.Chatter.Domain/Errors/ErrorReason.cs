namespace Bug.Chatter.Domain.Errors
{
	public static class ErrorReason
	{
		public static class GuidId
		{
			public const string IdRequired = "{0}: Id de recurso deve ser fornecido";
			public const string InvalidIdLoaded = "{0}: Id inválido ao carregar recurso";
		}

		public static class UserCode
		{
			public const string VerificationCodeRequired = "{0}: Código numérico deve ser fornecido";
			public const string VerificationCodeInvalid = "{0}: Informe um código válido. Ex: 123456";
			public const string MaxAttemptsToGenerateCodeReached = "Tentativas de gerar código de verificação foram excedidas, tente novamente mais tarde";
			public const string StatusRequired = "{0}: Status do código de verificação deve ser fornecido";
			public const string StatusInvalid = "{0}: Status do código de verificação inválido. Status: {1}";
			public const string NotFound = "{0}: Código de verificação '{1}' não encontrado";
			public const string Expired = "{0}: Código de verificação expirado";
			public const string GenerateCodeRetry = "Tentativa {0}: Código {1} já existe";
		}

		public static class User
		{
			public const string NameRequired = "{0}: Nome do usuário deve ser fornecido";
			public const string NameTooLarge = "{0}: Nome do usuário não pode exceder {1} caracteres";
			public const string PhoneNumberRequired = "{0}: Número de celular do usuário deve ser fornecido";
			public const string PhoneNumberInvalid = "{0}: Informe um número de celular válido. Ex: +55 (11) 91234-5678";
			public const string PhoneNumberMustBeUnique = "{0}: Número de celular '{1}' já cadastrado";
			public const string NotFoundByPhoneNumber = "{0}: Nenhum usuário foi encontrado com o número de celular '{1}'";
			public const string NotFoundById = "{0}: Nenhum usuário foi encontrado com o id '{1}'";
			public const string OnlyDraftUsersCanBeRegistered = "{0}: Apenas usuários pré-cadastrados podem ser registrados. Status atual: '{1}'";
		}

		public static class  Message
		{
			public const string ContentRequired = "{0}: Conteúdo da mensagem deve ser fornecido";
			public const string ContentTooLarge = "{0}: Conteúdo da mensagem não pode exceder {1} caracteres";
		}
	}
}
