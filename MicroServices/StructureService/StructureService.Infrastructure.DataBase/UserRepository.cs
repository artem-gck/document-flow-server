﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;
using StructureService.Infrastructure.DataBase.Context;

namespace StructureService.Infrastructure.DataBase
{
    public class UserRepository : IUserRepository
    {
        private readonly StructureContext _structureContext;

        public UserRepository(StructureContext structureContext)
        {
            _structureContext = structureContext ?? throw new ArgumentNullException(nameof(structureContext));
        }
        public async Task<Guid> AddAsync(Guid departmentId, UserEntity entity)
        {
            entity.Department = await GetDepartment(departmentId);
            entity.Position = await GetPosition(entity.Position.Id);

            var entityDb = _structureContext.Users.Add(entity);

            await _structureContext.SaveChangesAsync();

            return entityDb.Entity.Id;
        }

        public async Task<Guid> AddAsync(Guid id)
        {
            var entityDb = _structureContext.Users.Add(new UserEntity()
            {
                Id = id
            });

            await _structureContext.SaveChangesAsync();

            return entityDb.Entity.Id;
        }

        public async Task DeleteAsync(Guid departmentId, Guid userId)
        {
            var departmentEntity = await GetDepartmentWithUsers(departmentId);

            var entity = departmentEntity.Users.FirstOrDefault(us => us.Id == userId);

            if (entity is null)
                throw new UserNotFoundException(userId);

            _structureContext.Users.Remove(entity);

            await _structureContext.SaveChangesAsync();
        }

        public async Task<UserEntity> GetAsync(Guid departmentId, Guid userId)
        {
            var departmentEntity = await GetDepartmentWithUsers(departmentId);

            var entity = departmentEntity.Users.FirstOrDefault(us => us.Id == userId);

            if (entity is null)
                throw new UserNotFoundException(userId);

            return entity;
        }

        public async Task<IEnumerable<UserEntity>> GetByDepartmentId(Guid departmentId)
        {
            var departmentEntity = await GetDepartmentWithUsers(departmentId);

            if (departmentEntity is null)
                throw new UserNotFoundException(departmentId);

            var entities = departmentEntity.Users;

            return entities;
        }

        public async Task UpdateAsync(Guid departmentId, Guid userId, UserEntity entity)
        {
            entity.Id = userId;
            entity.Department = await GetDepartment(departmentId);
            entity.Position = await GetPosition(entity.Position.Id);

            _structureContext.Users.Update(entity);

            await _structureContext.SaveChangesAsync();
        }

        private async Task<DepartmentEntity> GetDepartment(Guid departmentId)
        {
            var entity = await _structureContext.Departments.FirstOrDefaultAsync(dep => dep.Id == departmentId);

            if (entity is null)
                throw new NotFoundException<DepartmentEntity>(departmentId);

            return entity;
        }

        private async Task<DepartmentEntity> GetDepartmentWithUsers(Guid departmentId)
        {
            var entity = await _structureContext.Departments.Include(dep => dep.Users)
                                                                .ThenInclude(us => us.Position)
                                                            .FirstOrDefaultAsync(dep => dep.Id == departmentId);

            if (entity is null)
                throw new NotFoundException<DepartmentEntity>(departmentId);

            return entity;
        }

        private async Task<PositionEntity> GetPosition(Guid id)
        {
            var entity = await _structureContext.Positions.FirstOrDefaultAsync(pos => pos.Id == id);

            if (entity is null)
                throw new NotFoundException<PositionEntity>(id);

            return entity;
        }
    }
}
