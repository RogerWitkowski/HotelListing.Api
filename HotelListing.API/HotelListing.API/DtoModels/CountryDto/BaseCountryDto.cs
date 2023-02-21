using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DtoModels.CountryDto
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }

        public string ShortName { get; set; }
    }
}