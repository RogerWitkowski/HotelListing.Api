namespace HotelListing.API.DtoModels.UserDto
{
    public class AuthenticationResponseDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}