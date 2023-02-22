using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DtoModels.UserDto
{
    public class ApiUserDto : ApiLoginUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}