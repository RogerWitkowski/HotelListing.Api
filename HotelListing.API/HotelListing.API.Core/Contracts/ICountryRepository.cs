using HotelListing.API.Data;
using HotelListing.API.DtoModels.CountryDto;

namespace HotelListing.API.Contracts;

public interface ICountryRepository : IGenericRepository<Country>
{
    Task<CountryDetailsDto> GetDetailsAsync(int id);
}