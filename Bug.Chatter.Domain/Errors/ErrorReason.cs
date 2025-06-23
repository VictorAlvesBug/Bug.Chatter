namespace Bug.Chatter.Domain.Errors
{
	public static class ErrorReason
	{
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
