﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UsersService.Services;
using UsersService.Services.Dto;
using UsersService.VewModels;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
            => (_userService, _mapper) = (userService, mapper);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetUsersInfoAsync()
            => Json((await _userService.GetUsersInfoAsync()).Select(us => _mapper.Map<UserInfoViewModel>(us)));

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> GetUserInfoAsync(int id)
        {
            var user = _mapper.Map<UserInfoViewModel>(await _userService.GetUserInfoAsync(id));

            return Json(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInfoAsync(int id)
        {
            var result = await _userService.DeleteUserInfoAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserInfoAsync(UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.AddUserInfoAsync(_mapper.Map<UserInfoDto>(userInfoViewModel));

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserInfoViewModel userInfoViewModel)
        {
            var result = await _userService.UpdateUserInfoAsync(_mapper.Map<UserInfoDto>(userInfoViewModel));

            return Ok(result);
        }
    }
}
