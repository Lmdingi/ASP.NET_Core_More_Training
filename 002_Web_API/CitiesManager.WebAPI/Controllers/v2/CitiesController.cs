using CitiesManager.WebAPI.DataBaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To get list of cities (only city name) from 'cities' table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetCities()
        {
            if (_context.Cities == null)
            {
                return NotFound();
            }

            var cities = await _context.Cities.OrderBy(temp => temp.CityName)
            .Select(temp => temp.CityName).ToListAsync();

            return cities;
        }
    }
}
