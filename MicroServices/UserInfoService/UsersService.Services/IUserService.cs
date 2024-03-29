﻿using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetUsersAsync();
        public Task<IEnumerable<UserDto>> GetUsersAsync(int page, int count);
        public Task<UserDto> GetUserAsync(Guid id);
        public Task DeleteUserAsync(Guid id);
        public Task<Guid> AddUserAsync(UserDto userDto);
        public Task UpdateUserAsync(Guid id, UserDto userDto);
        public Task<IEnumerable<UserDto>> GetUsersByDepartmentId(Guid departmentId);
    }
}
