namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	public interface IUserContext : IDynamoDbRepository<UserDTO>
	{
	}
}
