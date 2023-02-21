using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.DtoModels.CountryDto;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _dbContext;
        private readonly IMapper _mapper;

        public CountriesController(HotelListingDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _dbContext.Countries.ToListAsync();
            var getCountryDto = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(getCountryDto);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailsDto>> GetCountry(int id)
        {
            var country = await _dbContext
                .Countries
                .Include(h => h.Hotels)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDetailsDto = _mapper.Map<CountryDetailsDto>(country);

            return Ok(countryDetailsDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest();
            }

            var country = await _dbContext
                .Countries
                .FindAsync(id);

            if (country is null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateCountryDto>> PostCountry(CreateCountryDto createCountryDto)
        {
            var country = _mapper.Map<Country>(createCountryDto);

            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, createCountryDto);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _dbContext.Countries.Remove(country);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _dbContext.Countries.Any(e => e.Id == id);
        }
    }
}