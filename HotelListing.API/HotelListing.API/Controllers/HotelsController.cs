using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.DtoModels.HotelDto;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        public HotelsController(IMapper mapper, IHotelRepository hotelRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            var hotelsDto = _mapper.Map<List<HotelDto>>(hotels);
            return Ok(hotelsDto);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            var hotel = await _hotelRepository.GetAsync(id);
            if (hotel is null)
            {
                return NotFound();
            }

            _mapper.Map(hotelDto, hotel);

            try
            {
                await _hotelRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto createHotelDto)
        {
            var hotel = _mapper.Map<Hotel>(createHotelDto);
            await _hotelRepository.AddAsync(hotel);

            var hotelDto = _mapper.Map<HotelDto>(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotelDto);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _hotelRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelRepository.Exists(id);
        }
    }
}