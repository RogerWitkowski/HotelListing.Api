using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.DtoModels.CountryDto;
using HotelListing.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _dbContext;
        private readonly IMapper _mapper;

        public CountryRepository(HotelListingDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CountryDetailsDto> GetDetailsAsync(int id)
        {
            //return await _dbContext
            //    .Countries
            //    .Include(h => h.Hotels)
            //    .FirstOrDefaultAsync(c => c.Id == id);

            var country = await _dbContext
                .Countries
                .Include(q => q.Hotels)
                .ProjectTo<CountryDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetDetailsAsync), id);
            }

            return country;
        }
    }
}