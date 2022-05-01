using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("search")]
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
               from tags in _context.Tag
               where tags.Tag1.Contains(text)
               select new {
                   tag = tags.Tag1
               };

            var result = await query.Distinct().ToListAsync();

            return result;
        }

        // GET: /Search/Tags/LaTeX
        [HttpGet("users/tags/{text}", Name = "getUsersByTag")]
        public async Task<ActionResult<IEnumerable<Object>>> GetUsersByTag(string text) {
            var query =
                from user in _context.User
                join tags in _context.Tag on user.UserId equals tags.User
                join loc in _context.Dhbw on user.Dhbw equals loc.Location
                where tags.Tag1.Contains(text)
                select new {
                    userId = user.UserId,
                    firstName = user.Firstname,
                    lastName = user.Lastname,
                    dhbw = user.Dhbw,
                    course = user.Course,
                    courseAbr = user.CourseAbr,
                    specialization = user.Specialization,
                    email = user.EmailPrefix + "@" + loc.EmailDomain,
                    city = user.City,
                    biography = user.Biography,
                    isVerified = user.IsVerified,
                    tmsCreated = user.TmsCreated
                };

            var result = await query.Distinct().Take(25).ToListAsync();

            return result;
        }

    }
}
