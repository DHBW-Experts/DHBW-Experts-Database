using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("Search")]
    [ApiController]
    public class SearchController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public SearchController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("Tags/{text}", Name = "GetTagsByText")]
        public async Task<ActionResult<Object>> GetTagsByText(string text) {
            if (!Functions.authenticate(_context, 0, "")) {
                return Unauthorized();
            }
            var query =
               from tags in _context.Tags
               where tags.Tag1.Contains(text)
               select new {
                   tag = tags.Tag1
               };

            var result = await query.Distinct().ToListAsync();

            return result;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("Users/Tags/{text}", Name = "GetUsersByTag")]
        public async Task<ActionResult<IEnumerable<UsersNotSensitive>>> GetUsersByTag(string text) {
            if (!Functions.authenticate(_context, 0, "")) {
                return Unauthorized();
            }
            var query =
               from user in _context.UsersNotSensitives
               join tags in _context.Tags on user.UserId equals tags.User
               where tags.Tag1.Contains(text)
               select user;

            var result = await query.Distinct().Take(25).ToListAsync();

            return result;
        }

    }
}
