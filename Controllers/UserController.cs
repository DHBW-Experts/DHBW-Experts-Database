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
    public class UserController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public UserController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/id/5
        //The user assosiated with the specified ID is returned, if found.
        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<UsersNotSensitive>> GetUser(int id) {
            var result = await _context.UsersNotSensitives.Where(e => e.UserId == id).FirstOrDefaultAsync();
            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        //GET: /Users/rfid/432a9e7b626c87f
        [HttpGet("rfid/{rfidId}")]
        public async Task<ActionResult<UsersNotSensitive>> GetUser(string rfidId) {
            var result = await _context.Users.Where(e => e.RfidId == rfidId).FirstOrDefaultAsync();
            if (result is not null) {
                int id = result.UserId;
                return await _context.UsersNotSensitives.Where(e => e.UserId == id).FirstOrDefaultAsync();
            }
            return NotFound();
        }

    }



}
