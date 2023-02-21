namespace HotelListing.API.DtoModels.CountryDto
{
    public class CountryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }
        public List<HotelDto.HotelDto> Hotels { get; set; }
    }
}