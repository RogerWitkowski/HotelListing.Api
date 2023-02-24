using HotelListing.API.DtoModels.UserDto;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);

        Task<AuthenticationResponseDto> Login(ApiLoginUserDto loginUserDto);

        Task<string> CreateRefreshToken();

        Task<AuthenticationResponseDto> VerifyRefreshToken(AuthenticationResponseDto request);
    }
}