using comp_584_server.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using WorldModel;

namespace comp_584_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController(Comp584Context context) : ControllerBase
    {
        

        // GET: api/Countries
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await context.Countries.ToListAsync();
        }
        [HttpGet("population")]
        public async Task<ActionResult<IEnumerable<CountryPopulation>>> GetCountryPopulation()
        {
            return await context.Countries.
                Select(c => new CountryPopulation
                {
                    ID = c.Id,
                    name = c.Name,
                    iso2 = c.Iso2,
                    iso3 = c.Iso3,
                    population = c.Cities.Sum(city => city.Population)
                }).ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }
        // Replace the method signature and return type for GetCountryPopulation(int id)
        [HttpGet("population/{id}")]
        public ActionResult<CountryPopulation> GetCountryPopulation(int id)
        {
           //var x = context.Countries.Select(country => new CountryPopulation
           // {
           //     ID = country.Id,
           //     name = country.Name,
           //     iso2 = country.Iso2,
           //     iso3 = country.Iso3,
           //     population = country.Cities.Sum(city => city.Population)
           // });

            

            return context.Countries.Select(country => new CountryPopulation
            {
                ID = country.Id,
                name = country.Name,
                iso2 = country.Iso2,
                iso3 = country.Iso3,
                population = country.Cities.Sum(city => city.Population)
            }).Single(c => c.ID == id);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            context.Entry(country).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            context.Countries.Add(country);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            context.Countries.Remove(country);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return context.Countries.Any(e => e.Id == id);
        }
    }
}
