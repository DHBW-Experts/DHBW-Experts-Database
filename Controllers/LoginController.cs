using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public LoginController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /login/test@email.de
        // TEMPORARLY: Get User by E-Mail Adress for login purposes
        [HttpGet("{text}", Name = "getUserByEmail")]
        public async Task<ActionResult<Object>> getUserByEmail(String text) {

            var query =
                from user in _context.Users
                join loc in _context.Dhbws on user.Dhbw equals loc.Location
                where (user.EmailPrefix + "@" + loc.EmailDomain) == text
                select new {
                    userId = user.UserId,
                    firstName = user.Firstname,
                    lastname = user.Lastname,
                    dhbw = user.Dhbw,
                    course = user.Course,
                    courseAbr = user.CourseAbr,
                    specialization = user.Specialization,
                    email = text,
                    city = user.City,
                    biographie = user.Bio,
                    isVerified = user.IsVerified,
                    tmsCreated = user.TmsCreated
                };

            var result = await query.FirstOrDefaultAsync();

            if (result is null) {
                return NotFound();
            }
            return result;
        }

    }



}
