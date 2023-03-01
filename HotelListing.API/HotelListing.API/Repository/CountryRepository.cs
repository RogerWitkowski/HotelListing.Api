using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
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

        public async Task<Country> GetDetailsAsync(int id)
        {
            return await _dbContext
                .Countries
                .Include(h => h.Hotels)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}