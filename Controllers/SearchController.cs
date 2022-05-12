using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseAPI.Controllers {
    [Route("search")]
    [ApiController]
    public class SearchController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public SearchController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("tags/{text}", Name = "getTagsByText")]
        [Authorize]
        public async Task<ActionResult<Object>> getTagsByText(string text) {
            var query =
               from tags in _context.Tags
               where tags.Tag.Contains(text)
               select new {
                   tag = tags.Tag
               };

            var result = await query.Distinct().Take(25).ToListAsync();

            return result;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("users/tags/{text}", Name = "getUsersByTag")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> GetUsersByTag(string text) {
            var query =
                from user in _context.VwUsers
                join tags in _context.Tags on user.UserId equals tags.User
                where tags.Tag.Contains(text)
                select user;

            var result = await query.Distinct().Take(25).ToListAsync();

            return result;
        }

        //GET: /Users/rfid/432a9e7b626c87f
        [HttpGet("users/rfid/{rfidId}")]
        [Authorize]
        public async Task<ActionResult<Object>> getUserByRfidId(string rfidId) {
            var query =
                from user in _context.VwUsers
                join data in _context.UserData on user.UserId equals data.User
                where data.RfidId == rfidId
                select user;

            var result = await query.FirstOrDefaultAsync();
            if (result is null) {
                return NotFound();
            }
            return result;
        }

    }

}
