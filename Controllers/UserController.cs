using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("Users")]
    [ApiController]
    public class RequestsController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public RequestsController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        //GET: /Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            var result = await _context.Users.ToListAsync();

            foreach (var item in Request.Headers) {
                Console.WriteLine(item.ToString());
            }

            return result;
        }

        // GET: /Users/5
        //The user assosiated with the specified ID is returned, if found.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await _context.Users.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            return user;
        }

    }



}
