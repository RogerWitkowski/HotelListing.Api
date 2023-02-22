using HotelListing.API.DtoModels.UserDto;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<bool> Register(ApiUserDto userDto);
    }
}