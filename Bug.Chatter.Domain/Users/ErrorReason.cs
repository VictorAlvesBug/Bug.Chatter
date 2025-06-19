namespace Bug.Chatter.Domain.Users
{
	public static class ErrorReason
	{
		public const string IdRequired = "{0}: Id do usuário deve ser fornecido";
		public const string NameRequired = "{0}: Nome do usuário deve ser fornecido";
		public const string PhoneNumberRequired = "{0}: Número de celular do usuário deve ser fornecido";
		public const string PhoneNumberInvalid = "{0}: Número de celular do usuário é inválido";
	}
}
