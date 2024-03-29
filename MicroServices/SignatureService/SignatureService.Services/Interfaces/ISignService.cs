﻿using SignatureService.Services.Dto;

namespace SignatureService.Services.Interfaces
{
    public interface ISignService
    {
        public Task AddAsync(Guid userId, Guid documentId, int version);
        public Task AddAsync(Guid userId, Guid documentId, int version, string token);
        public Task<IEnumerable<UserPublicKey>> GetUsersByDocumentIdAsync(Guid id, int version);
        public Task<bool> CheckDocumentByUserAsync(Guid documentId, int version, byte[] publicKey);
        public Task<bool> CheckDocumentByUserAsync(Guid documentId, int version, byte[] publicKey, string token);
    }
}
