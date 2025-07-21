using Bug.Chatter.Domain.Common.ValueObjects;
using Bug.Chatter.Domain.SeedWork.Specifications.UserLoad;
using Bug.Chatter.Domain.Users;
using Bug.Chatter.Domain.Users.Entities;
using Bug.Chatter.Domain.Users.ValueObjects;
using Bug.Chatter.Infrastructure.Persistence.DynamoDb.Configurations;

namespace Bug.Chatter.Infrastructure.Persistence.DynamoDb.Users
{
	internal class UserRepository : IUserRepository
	{
		private readonly IDynamoDbRepository<UserDTO> _userContext;
		private readonly IDynamoDbRepository<UserCodeDTO> _codeContext;

		public UserRepository(IDynamoDbRepository<UserDTO> userContext, IDynamoDbRepository<UserCodeDTO> codeContext)
		{
			_userContext = userContext;
			_codeContext = codeContext;
		}

		public async Task<User?> GetByUserIdAsync(GuidId id, IUserLoadSpecification spec)
		{
			ArgumentNullException.ThrowIfNull(id, nameof(id));

			var userDto = await _userContext.GetAsync(KeyFactory.UserPk(id), KeyFactory.UserSk());
			if (userDto is null) return null;

			if (spec.IncludeVerificationCodes)
				return await IncludeVerificationCodes(userDto);

			return userDto.ToDomain([]);
		}

		public async Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber, IUserLoadSpecification spec)
		{
			ArgumentNullException.ThrowIfNullOrWhiteSpace(phoneNumber.Value, nameof(phoneNumber));

			var userDto = (await _userContext.ListByIndexKeysAsync(DatabaseSettings.PhoneNumberSkIndex, phoneNumber.Value, KeyFactory.UserSk()))
				?.FirstOrDefault();
			if (userDto is null) return null;

			if (spec.IncludeVerificationCodes)
				return await IncludeVerificationCodes(userDto);

			return userDto.ToDomain([]);
		}

		public async Task<IEnumerable<User>> ListByIdsAsync(IEnumerable<GuidId> ids, IUserLoadSpecification spec)
		{
			ArgumentNullException.ThrowIfNull(ids, nameof(ids));

			if (!ids.Any())
				return [];

			var keysToGetUsers = ids.Select(id => (KeyFactory.UserPk(id), KeyFactory.UserSk()));

			var userDtos = await _userContext.BatchGetAsync(keysToGetUsers);

			var userMissingKeys = MissingKeys(keysToGetUsers, userDtos);

			if (userMissingKeys.Any())
			{
				var strMissingKeys = string.Join(", ", userMissingKeys.Select(mk => $"(PK: {mk.pk} e SK: {mk.sk})"));

				throw new Exception(
					$"Alguns usuários não foram encontrados. PKs não encontradas: {strMissingKeys}");
			}

			if (spec.IncludeVerificationCodes)
				return await IncludeVerificationCodes(userDtos);

			return userDtos.Select(userDto => userDto.ToDomain(codeDtos: [])).ToArray();
		}

		public async Task SaveAsync(User user, IUserLoadSpecification spec)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			var exists = await _userContext.GetAsync(KeyFactory.UserPk(user.Id), KeyFactory.UserSk()) is not null;

			if (exists)
			{
				await UpdateAsync(user, spec);
				return;
			}

			await CreateAsync(user, spec);
		}

		public async Task DeleteAsync(User user)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			var codes = await _codeContext.QueryAsync(KeyFactory.CodePk(user.Id), skBeginsWith: $"{KeyFactory.CodePrefix}-");

			await Task.WhenAll(
				codes.Select(code =>
					_codeContext.DeleteAsync(code.PK, code.SK, code.Version))
			);

			await _userContext.DeleteAsync(KeyFactory.UserPk(user.Id), KeyFactory.UserSk(), user.Version);
		}

		private async Task CreateAsync(User user, IUserLoadSpecification spec)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			await _userContext.SafePutAsync(user.ToDTO());

			if (spec.IncludeVerificationCodes)
			{
				await Task.WhenAll(
					user.Codes.Select(async code => await _codeContext.SafePutAsync(code.ToDTO(user)))
				);
			}
		}

		private async Task UpdateAsync(User user, IUserLoadSpecification spec)
		{
			ArgumentNullException.ThrowIfNull(user, nameof(user));

			var userVersion = user.Version;
			user.IncrementVersion();
			await _userContext.UpdateDynamicAsync(user.ToDTO(), userVersion);

			if (spec.IncludeVerificationCodes)
			{
				var dbCodeDtos = await _codeContext.QueryAsync(KeyFactory.CodePk(user.Id), skBeginsWith: $"{KeyFactory.CodePrefix}-");

				await Task.WhenAll(
					dbCodeDtos.Select(async dbCodeDto => await _codeContext.DeleteAsync(dbCodeDto.PK, dbCodeDto.SK, dbCodeDto.Version))
				);

				await Task.WhenAll(
					user.Codes.Select(async code =>
					{
						code.IncrementVersion();
						await _codeContext.SafePutAsync(code.ToDTO(user));
					})
				);
			}
		}

		private static IEnumerable<(string pk, string sk)> MissingKeys(IEnumerable<(string pk, string sk)> keysToGet, IEnumerable<UserDTO> dtos)
		{
			return keysToGet.Except(dtos.Where(x => x is not null).Select(x => (x.PK, x.SK)));
		}

		private async Task<IEnumerable<User>> IncludeVerificationCodes(IEnumerable<UserDTO> userDtos)
		{
			return (await Task.WhenAll(userDtos.Select(async userDto => await IncludeVerificationCodes(userDto)))).ToList();
		}

		private async Task<User> IncludeVerificationCodes(UserDTO userDto)
		{
			var codeDtos = await _codeContext.QueryAsync(userDto.PK, skBeginsWith: $"{KeyFactory.CodePrefix}-");
			return userDto.ToDomain(codeDtos);
		}
	}
}
