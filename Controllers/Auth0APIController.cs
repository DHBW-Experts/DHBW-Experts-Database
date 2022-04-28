using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("auth0")]
    [ApiController]
    public class Auth0APIController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public Auth0APIController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        public class UserObject {
            public string auth0UserId { get; set; }
            public string email { get; set; }
            public DateTime createdAt { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(UserObject userIn) {
            Auth0User user = new Auth0User();
            user.Auth0UserId = userIn.auth0UserId;
            user.EmailPrefix = userIn.email.Split('@')[0];
            user.EmailDomain = userIn.email.Split('@')[1];
            user.CreatedAt = userIn.createdAt;

            Console.WriteLine(userIn.auth0UserId);
            Console.WriteLine(userIn.email);
            Console.WriteLine(userIn.createdAt);

            _context.Auth0User.Add(user);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        public class DomainObject {
            public string domain { get; set; }
        }

        [HttpGet("checkdomain")]
        public async Task<Object> isDomainValid([FromBody]DomainObject domainObject) {

            var query =
                from dhbw in _context.Auth0Dhbw
                where dhbw.Domain == domainObject.domain
                select dhbw;


            var result = await query.CountAsync();

            if (result is 0) {
                return new {
                    domain = domainObject.domain,
                    isValid = false
                };
            }
            return new {
                domain = domainObject.domain,
                isValid = true
            };

        }
    }
}
