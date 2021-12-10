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
        [HttpGet("id/{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UsersNotSensitive>> GetUserById(int id) {
            if (!Functions.authenticate(_context, 0, "")) {
                return Unauthorized();
            }
            var result = await _context.UsersNotSensitives.Where(e => e.UserId == id).FirstOrDefaultAsync();
            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        //GET: /Users/rfid/432a9e7b626c87f
        [HttpGet("rfid/{rfidId}")]
        public async Task<ActionResult<UsersNotSensitive>> GetUser(string rfidId) {
            if (!Functions.authenticate(_context, 0, "")) {
                return Unauthorized();
            }
            var result = await _context.Users.Where(e => e.RfidId == rfidId).FirstOrDefaultAsync();
            if (result is not null) {
                int id = result.UserId;
                return await _context.UsersNotSensitives.Where(e => e.UserId == id).FirstOrDefaultAsync();
            }
            return NotFound();
        }

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/contacts", Name = "GetContactsByUserId")]
        public async Task<ActionResult<IEnumerable<UsersNotSensitive>>> GetContacsByUserID(int id) {

            var query =
                from contact in _context.Contacts
                join user in _context.UsersNotSensitives on contact.Contact1 equals user.UserId
                where contact.User == id
                select user;

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpPost("{id:int}/contacts/add/{idContact:int}", Name = "AddContactToUser")]
        public async Task<ActionResult<IEnumerable<UsersNotSensitive>>> AddContactToUser(int id, int idContact) {

            if (id == idContact) {
                return BadRequest();
            }

            Contact contact = new Contact();

            contact.User = id;
            contact.Contact1 = idContact;

            _context.Contacts.Add(contact);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        // GET: /Users/5/tags
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/tags", Name = "GetTagsByUserId")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTagsByUserID(int id) {

            var query =
                from tag in _context.Tags
                where tag.User == id
                select tag;

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        // POST: /Users/5/tags
        //The user assosiated contacts of the user a returned
        [HttpPost("{id:int}/tags/add/{text}", Name = "AddTagToUser")]
        public async Task<IActionResult> AddTagToUser(int id, string text) {

            Tag tag = new Tag();

            tag.User = id;
            tag.Tag1 = text;

            _context.Tags.Add(tag);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        [HttpPost("{id:int}/edit", Name = "editUser")]
        public async Task<IActionResult> editUser(User editedUser) {

            var user = _context.Users.FirstOrDefault(u => u.UserId == editedUser.UserId);

            Console.WriteLine(editedUser);
            Console.WriteLine("CourseAbr: " + editedUser.CourseAbr);
            Console.WriteLine("Course: " + editedUser.Course);
            Console.WriteLine("Specialization: " + editedUser.Specialization);
            Console.WriteLine("City: " + editedUser.City);
            Console.WriteLine("Bio: " + editedUser.Bio);
            Console.WriteLine("RfidId: " + editedUser.RfidId);
            Console.WriteLine("PwHash: " + editedUser.PwHash);
            Console.WriteLine("IsVerified: " + editedUser.IsVerified);
            Console.WriteLine("TmsCreated" + editedUser.TmsCreated == null);
            Console.WriteLine("PwHash: " + editedUser.PwHash);
            Console.WriteLine("VerificationId: " + editedUser.VerificationId);
            Console.WriteLine(editedUser.Firstname == null);

            bool isValid = (
                (user.VerificationId == editedUser.VerificationId || editedUser.VerificationId == 0) &&
                (user.IsVerified == editedUser.IsVerified || editedUser.IsVerified == false) &&
                (user.Firstname == editedUser.Firstname || editedUser.Firstname == null) &&
                (user.Lastname == editedUser.Lastname || editedUser.Lastname == null) &&
                (user.EmailPrefix == editedUser.EmailPrefix || editedUser.EmailPrefix == null) &&
                (user.Dhbw == editedUser.Dhbw || editedUser.Dhbw == null) &&
                (user.TmsCreated == editedUser.TmsCreated || editedUser.TmsCreated == null)
            );

            Console.WriteLine(isValid);

            if (!isValid) {
                return Conflict("Some data provied can't be changed");
            }

            if (editedUser.Course != null) {
                user.Course = editedUser.Course;
            }
            if (editedUser.CourseAbr != null) {
                user.CourseAbr = editedUser.CourseAbr;
            }
            if (editedUser.Specialization != null) {
                user.Specialization = editedUser.Specialization;
            }
            if (editedUser.City != null) {
                user.City = editedUser.CourseAbr;
            }
            if (editedUser.Bio != null) {
                user.Bio = editedUser.Bio;
            }
            if (editedUser.RfidId != null) {
                user.RfidId = editedUser.RfidId;
            }
            if (editedUser.PwHash != null) {
                user.PwHash = editedUser.PwHash;
            }

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }



}
