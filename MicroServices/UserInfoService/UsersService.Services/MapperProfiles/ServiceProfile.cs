﻿using AutoMapper;
using UsersService.DataAccess.Entities;
using UsersService.Services.Dto;

namespace UsersService.Services.MapperProfiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<UserInfoEntity, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfoEntity>();
        }
    }
}
