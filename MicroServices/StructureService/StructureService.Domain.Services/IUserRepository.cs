﻿using StructureService.Domain.Entities;

namespace StructureService.Domain.Services
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserEntity>> GetByDepartmentId(Guid departmentId);
        public Task<UserEntity> GetAsync(Guid departmentId, Guid userId);
        public Task<Guid> AddAsync(Guid departmentId, UserEntity entity);
        public Task<Guid> AddAsync(Guid id);
        public Task DeleteAsync(Guid departmentId, Guid userId);
        public Task UpdateAsync(Guid departmentId, Guid userId, UserEntity entity);
    }
}
