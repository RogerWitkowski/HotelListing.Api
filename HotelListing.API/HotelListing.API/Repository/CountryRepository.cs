using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _dbContext;

        public CountryRepository(HotelListingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
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