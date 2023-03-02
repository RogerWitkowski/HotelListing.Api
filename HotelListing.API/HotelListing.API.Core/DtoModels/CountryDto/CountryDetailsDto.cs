namespace HotelListing.API.DtoModels.CountryDto
{
    public class CountryDetailsDto : BaseCountryDto
    {
        public int Id { get; set; }

        public List<HotelDto.HotelDto> Hotels { get; set; }
    }
}