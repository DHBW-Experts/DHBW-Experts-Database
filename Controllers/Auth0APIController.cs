using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;
using Microsoft.AspNetCore.Authorization;

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
        }

        [HttpPost("register")]
        [Authorize("write:auth0-api")]
        public async Task<IActionResult> register(UserObject userIn) {
            Users user = new Users();
            user.UserId = userIn.auth0UserId;
            user.EmailPrefix = userIn.email.Split('@')[0];
            user.EmailDomain = userIn.email.Split('@')[1];

            _context.Users.Add(user);

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
        [Authorize("read:auth0-api")]
        public async Task<Object> isDomainValid([FromBody]DomainObject domainObject) {

            var query =
                from dhbw in _context.DhbwDomains
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
