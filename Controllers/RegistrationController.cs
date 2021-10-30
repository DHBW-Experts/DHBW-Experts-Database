using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("Register")]
    [ApiController]
    public class RegistrationController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public RegistrationController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> register(User user) {
            return BadRequest("not yet Implemented: registration");
        }

        [HttpPut("{userId:int}/{verificationId:int}")]
        public async Task<IActionResult> verify(int userId, int verificationId) {
            Console.WriteLine(userId);
            Console.WriteLine(verificationId);
            User toBeVerified = await _context.Users.FindAsync(userId);
            if (toBeVerified is not null && !toBeVerified.IsVerified) {
                int expectedID = toBeVerified.VerificationId;

                if (verificationId == expectedID) {
                    await verify(toBeVerified);
                    Console.Write("successfully verified: user {0}, id {1}", userId, verificationId);
                    return Ok();
                }
                Console.Write("verification rejected: wrong id! user {0}, id {1}", userId, verificationId);
                return BadRequest();
            }
            Console.Write("verification rejected: user not found or already verified! user {0}, id {1}", userId, verificationId);
            return BadRequest();
        }

        private async Task verify(User user) {
            user.IsVerified = true;

            _context.Users.Update(user);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException e) {
                Console.WriteLine(e);
            }
        }

    }
}
