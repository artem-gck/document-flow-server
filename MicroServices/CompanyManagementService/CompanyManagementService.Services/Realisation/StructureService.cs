﻿using AutoMapper;
using CompanyManagementService.Cache;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.Realisation;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.Services.Dto;
using CompanyManagementService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace CompanyManagementService.Services.Realisation
{
    public class StructureService : IStructureService
    {
        private readonly IUserInfoAccess _userInfoAccess;
        private readonly IUserStructureAccess _userStructureAccess;
        private readonly IPositionsAccess _positionsAccess;
        private readonly IDepartmentAccess _departmentAccess;
        private readonly IDistributedCache _cache;
        private readonly ILogger<StructureService> _logger;
        private readonly IMapper _mapper;

        public StructureService(IUserInfoAccess userInfoAccess, IUserStructureAccess userStructureAccess, IPositionsAccess positionsAccess, IDepartmentAccess departmentAccess, IDistributedCache cache, IMapper mapper, ILogger<StructureService> logger)
        {
            _userInfoAccess = userInfoAccess ?? throw new ArgumentNullException(nameof(userInfoAccess));
            _userStructureAccess = userStructureAccess ?? throw new ArgumentNullException(nameof(userStructureAccess));
            _positionsAccess = positionsAccess ?? throw new ArgumentNullException(nameof(positionsAccess));
            _departmentAccess = departmentAccess ?? throw new ArgumentNullException(nameof(departmentAccess));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CheifStructureDto> GetCheifStructureAsync(Guid cheifId, string token)
        {
            var cacheId = $"Cheif_{cheifId}";
            var cheifStructureDto = await _cache.GetRecordAsync<CheifStructureDto>(cacheId);

            if (cheifStructureDto is null)
            {
                var cheif = await _userInfoAccess.GetAsync(cheifId, token);
                var cheifInfo = await _userStructureAccess.GetAsync(cheif.DepartmentId.Value, cheifId, token);

                var users = (await _userInfoAccess.GetByDepartmentIdAsync(cheif.DepartmentId.Value, token));
                var usersInfo = (await _userStructureAccess.GetByDepartmentIdAsync(cheif.DepartmentId.Value, token));

                var listOfUsersInfo = usersInfo.Zip(users).ToList();

                var cheifDto = _mapper.Map<UserDto>((cheifInfo, cheif));
                var usersDto = _mapper.Map<IEnumerable<UserDto>>(listOfUsersInfo);

                cheifStructureDto = new CheifStructureDto()
                {
                    Department = cheifInfo.Department,
                    Cheif = cheifDto,
                    Subordinates = usersDto
                };

                await _cache.SetRecordAsync(cacheId, cheifStructureDto);
            }

            return cheifStructureDto;
        }

        public async Task<CheifStructureDto> GetCheifStructureAsync(Guid cheifId)
            => await GetCheifStructureAsync(cheifId, null);

        public async Task<UserDto> GetUserAsync(Guid userId)
            => await GetUserAsync(userId, null);

        public async Task<UserDto> GetUserAsync(Guid userId, string token)
        {
            var cacheId = $"User_{userId}";
            var userDto = await _cache.GetRecordAsync<UserDto>(cacheId);

            if (userDto is null)
            {
                var user = await _userInfoAccess.GetAsync(userId, token);

                PositionResponce position;

                if (user.PositionId.HasValue)
                    position = await _positionsAccess.GetAsync(user.PositionId.Value, token);
                else position = new PositionResponce()
                {
                    Name = null
                };

                DepartmentResponce department;

                if (user.DepartmentId.HasValue)
                    department = await _departmentAccess.GetAsync(user.DepartmentId.Value, token);
                else department = new DepartmentResponce()
                {
                    Name = null
                };

                //var position = await _positionsAccess.GetAsync(user.PositionId.Value, token);
                //var department = await _departmentAccess.GetAsync(user.DepartmentId.Value, token);

                userDto = _mapper.Map<UserDto>((position, department, user));
                await _cache.SetRecordAsync(cacheId, userDto);
            }

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByDepartmentAsync(Guid departmentId)
            => await GetUsersByDepartmentAsync(departmentId, null);

        public async Task<IEnumerable<UserDto>> GetUsersByDepartmentAsync(Guid departmentId, string token)
        {
            var users = (await _userInfoAccess.GetByDepartmentIdAsync(departmentId, token));
            var usersInfo = (await _userStructureAccess.GetByDepartmentIdAsync(departmentId, token));

            var listOfUsersInfo = usersInfo.Zip(users).ToList();

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(listOfUsersInfo);

            return usersDto;
        }
    }
}
