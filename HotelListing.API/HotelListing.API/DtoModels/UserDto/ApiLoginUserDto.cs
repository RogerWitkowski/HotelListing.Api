using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DtoModels.UserDto
{
    public class ApiLoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}