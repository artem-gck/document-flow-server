﻿using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;

namespace SignatureService.DataAccess.DataBase.Realisations
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerConnectionProvider _provider;

        public UserRepository(SqlServerConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<Guid> AddUserAsync(UserEntity user)
        {
            var sql = $"INSERT INTO users (Id, PublicKey, PrivateKey) " +
                      $"VALUES ('{user.Id}', @Public, @Private)";

            using var connection = _provider.GetDbConnection();

            await connection.ExecuteAsync(sql, new { Public = user.PublicKey, Private = user.PrivateKey});

            var b = "SELECT * FROM users";

            var a = await connection.QueryAsync<UserEntity>(b);

            return user.Id;
        }
    }
}
