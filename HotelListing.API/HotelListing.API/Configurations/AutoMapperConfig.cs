using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.DtoModels.CountryDto;

namespace HotelListing.API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
        }
    }
}