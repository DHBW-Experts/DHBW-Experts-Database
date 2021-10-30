using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Script;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BirdsController : ControllerBase
    {
        private readonly BirdTrackerMSSQLContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public BirdsController(BirdTrackerMSSQLContext context)
        {
            _context = context;
        }

        //GET: Birds
        //Birds are returned sorted by longitutde and latitude. A stable sorting algorithm is used so the sorts can be queued like done here.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bird>>> GetBirds()
        {
            var result = await _context.Birds.ToListAsync();
            result.Sort((b1, b2) => Decimal.Compare((decimal)b1.Longitude, (decimal)b2.Longitude));
            result.Sort((b1, b2) => Decimal.Compare((decimal)b1.Latitude, (decimal)b2.Latitude));
            return result;
        }
        //GET: Birds/Amsel
        //The specific bird type is returned sorted by longitutde and latitude. A stable sorting algorithm is used so the sorts can be queued like done here.
        [HttpGet("{species}")]
        public async Task<ActionResult<IEnumerable<Bird>>> GetBirdsBySpecies(string species)
        {
            var result = await _context.Birds.Where(b => b.Species == species).ToListAsync();
            result.Sort((b1, b2) => Decimal.Compare((decimal)b1.Longitude, (decimal)b2.Longitude));
            result.Sort((b1, b2) => Decimal.Compare((decimal)b1.Latitude, (decimal)b2.Latitude));
            return result;
        }

        // GET: Birds/5
        //The bird assosiated with the specified ID is returned, if found.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Bird>> GetBird(int id)
        {
            var bird = await _context.Birds.FindAsync(id);

            if (bird == null)
            {
                return NotFound();
            }

            return bird;
        }

        // PUT: Birds/5
        // Entries in the DB can be updated, ensures the given bird matches the given Id.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBird(int id, Bird bird)
        {
            if (id != bird.Id)
            {
                return BadRequest();
            }

            _context.Entry(bird).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BirdExists(id))
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

        // POST: Birds
        // Makes a new entry and saves it to the DB. A Bird must have an ID, longitutde and latitude, otherwise a Bad Request is returned.
        [HttpPost]
        public async Task<ActionResult<Bird>> PostBird(Bird bird)
        {
            bird.Id = GenerateNewId();

            if (bird.Latitude == null || bird.Longitude == null) return BadRequest();
            _context.Birds.Add(bird);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BirdExists(bird.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBird", new { id = bird.Id }, bird);
        }

        // DELETE: Birds/5
        // The bird assosiated with the given Id is deleted from the DB.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBird(int id)
        {
            var bird = await _context.Birds.FindAsync(id);
            if (bird == null)
            {
                return NotFound();
            }

            _context.Birds.Remove(bird);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BirdExists(int id)
        {
            return _context.Birds.Any(e => e.Id == id);
        }
        //returns the highest free Id in the DB.
        private int GenerateNewId()
        {
            return _context.Birds.Max(b => b.Id) + 1;
        }
        //Script used to populate the DB with random Birdobjects.
        private void FillDB(int numberOfBird)
        {
            int startId = GenerateNewId();

            for (int i = 0; i < numberOfBird; i++)
            {
                Bird bird = GenerateBirds.randomBird(startId);
                _context.Birds.Add(bird);
                startId++;
            }
            _context.SaveChanges();
        }
    }
}
