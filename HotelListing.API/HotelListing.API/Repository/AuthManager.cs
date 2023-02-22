using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.DtoModels.UserDto;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }

        public async Task<bool> Login(ApiLoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if (user is null)
            {
                return default;
            }

            bool isValidCredentials = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

            if (!isValidCredentials)
            {
                return default;
            }

            return true;

            //try
            //{
            //    var userq = await _userManager.FindByEmailAsync(loginUserDto.Email);
            //    isValidUser = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            //}
            //catch (Exception)
            //{
            //}

            //return isValidUser;
        }
    }
}