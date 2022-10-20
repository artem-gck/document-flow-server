﻿using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;

namespace SignatureService.DataAccess.DataBase.Realisations
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly SqlServerConnectionProvider _provider;

        public SignatureRepository(SqlServerConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task AddAsync(SignatureEntity entity)
        {
            var sql = $"INSERT INTO signature (DocumentId, Version, Hash, UserId) " +
                      $"VALUES ('{entity.DocumentId}', {entity.Version}, @Hash, '{entity.UserId}')";

            using var connection = _provider.GetDbConnection();

            await connection.ExecuteAsync(sql, new { Hash = entity.Hash });
        }

        public async Task<IEnumerable<SignatureEntity>> GetByDocumentIdAsync(Guid id, int version)
        {
            var sql = $"SELECT * " +
                      $"FROM signature " +
                      $"WHERE DocumentId = '{id}' AND Version = {version}";

            using var connection = _provider.GetDbConnection();

            var signatureEntity = await connection.QueryAsync<SignatureEntity>(sql);

            return signatureEntity;
        }

        public async Task<SignatureEntity> GetSignatureAync(Guid userId, Guid documentId, int version)
        {
            var sql = $"SELECT * " +
                      $"FROM signature " +
                      $"WHERE UserId = {userId} AND DocumentId = '{documentId}' AND Version = {version}";

            using var connection = _provider.GetDbConnection();

            var signatureEntity = await connection.QueryFirstOrDefaultAsync<SignatureEntity>(sql);

            return signatureEntity;
        }
    }
}
