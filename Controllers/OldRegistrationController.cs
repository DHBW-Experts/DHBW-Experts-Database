using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("old/register")]
    [ApiController]
    public class OldRegistrationController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public OldRegistrationController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> registerOld(User user) {
            if (user.VerificationId != 0 || user.UserId != 0 || user.IsVerified != false) {
                return Conflict("Data provided will be set automaticly");
            }
            user.VerificationId = Functions.generateVerificationCode();
            _context.User.Add(user);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            var result = new {
                userId = user.UserId,
                firstName = user.Firstname,
                lastname = user.Lastname,
                dhbw = user.Dhbw,
                course = user.Course,
                courseAbr = user.CourseAbr,
                specialization = user.Specialization,
                email = user.EmailPrefix + "@" + (await _context.Dhbw.FindAsync(user.Dhbw)).EmailDomain,
                city = user.City,
                biography = user.Biography,
                isVerified = user.IsVerified,
                tmsCreated = user.TmsCreated
            };

            return CreatedAtRoute("getUserByIdOld", new { id = user.UserId }, result);
        }

        [HttpPut("{userId:int}/{verificationId:int}")]
        public async Task<IActionResult> verifyOld(int userId, int verificationId) {
            User toBeVerified = await _context.User.FindAsync(userId);
            if (toBeVerified is not null && !toBeVerified.IsVerified) {
                int expectedID = toBeVerified.VerificationId;

                if (verificationId == expectedID) {
                    toBeVerified.IsVerified = true;

                    _context.User.Update(toBeVerified);

                    try {
                        await _context.SaveChangesAsync();
                    } catch (DbUpdateConcurrencyException e) {
                        Console.WriteLine(e);
                    }
                    Console.Write("successfully verified: user {0}, id {1}", userId, verificationId);
                    return Ok();
                }
                Console.Write("verification rejected: wrong id! user {0}, id {1}", userId, verificationId);
                return BadRequest();
            }
            Console.Write("verification rejected: user not found or already verified! user {0}, id {1}", userId, verificationId);
            return BadRequest();
        }
    }
}
