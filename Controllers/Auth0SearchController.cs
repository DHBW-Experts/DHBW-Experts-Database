using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("auth0-search")]
    [ApiController]
    public class Auth0SearchController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public Auth0SearchController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("tags/{text}", Name = "getTagsByText")]
        public async Task<ActionResult<Object>> getTagsByText(string text) {
            var query =
               from tags in _context.Auth0Tags
               where tags.Tag.Contains(text)
               select new {
                   tag = tags.Tag
               };

            var result = await query.Distinct().Take(25).ToListAsync();

            return result;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("users/tags/{text}", Name = "getUsersByTag")]
        public async Task<ActionResult<IEnumerable<Object>>> GetUsersByTag(string text) {
            var query =
                from user in _context.VwUsers
                join tags in _context.Auth0Tags on user.UserId equals tags.User
                where tags.Tag.Contains(text)
                select user;

            var result = await query.Distinct().Take(25).ToListAsync();

            return result;
        }

    }
}
