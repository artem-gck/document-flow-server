﻿using TaskCrudService.Application.Services.Dto;

namespace TaskCrudService.Application.Services
{
    public interface IService<T> where T : BaseDto
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<Guid> AddAsync(T dto);
        public Task<Guid> DeleteAsync(Guid id);
        public Task<Guid> UpdateAsync(Guid id, T dto);
    }
}