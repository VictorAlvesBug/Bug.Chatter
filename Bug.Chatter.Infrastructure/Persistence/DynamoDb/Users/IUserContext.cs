namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public interface IUserContext : IDynamoDbRepository<UserDTO>
	{
		// Métodos específicos de usuário
	}
}
