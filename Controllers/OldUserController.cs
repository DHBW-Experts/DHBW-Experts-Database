using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("old/users")]
    [ApiController]
    public class OldUserController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public OldUserController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/5
        //The user assosiated with the specified ID is returned, if found.
        [HttpGet("{id:int}", Name = "getUserByIdOld")]
        public async Task<ActionResult<Object>> getUserByIdOld(int id) {
            var query =
                from user in _context.User
                join loc in _context.Dhbw on user.Dhbw equals loc.Location
                where user.UserId == id
                select new {
                    userId = id,
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


            var result = await query.FirstOrDefaultAsync();
            if (result is null) {
                return NotFound();
            }
            return result;
        }

        //GET: /Users/rfid/432a9e7b626c87f
        [HttpGet("rfid/{rfidId}")]
        public async Task<ActionResult<Object>> getUserOld(string rfidId) {
            var query =
                from user in _context.User
                join loc in _context.Dhbw on user.Dhbw equals loc.Location
                where user.RfidId == rfidId
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


            var result = await query.FirstOrDefaultAsync();
            if (result is null) {
                return NotFound();
            }
            return result;
        }

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/contacts", Name = "getContactsByUserIdOld")]
        public async Task<ActionResult<IEnumerable<Object>>> getContacsByUserIDOld(int id) {

            var query =
                from contact in _context.Contact
                join user in _context.User on contact.Contact1 equals user.UserId
                join loc in _context.Dhbw on user.Dhbw equals loc.Location
                where contact.User == id
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

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id:int}/contacts/{contactId}", Name = "deleteContactByUserIdOld")]
        public async Task<ActionResult> deleteContactByUserIdOld(int id, int contactId) {

            var query =
                from contact in _context.Contact
                where contact.User == id && contact.Contact1 == contactId
                select contact;

            var c = await query.FirstOrDefaultAsync();

            if (c is null) {
                return NotFound();
            }

            _context.Contact.Remove(c);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpPost("{id:int}/contacts/add/{idContact:int}", Name = "addContactToUserOld")]
        public async Task<ActionResult> addContactToUserOld(int id, int idContact) {

            if (id == idContact) {
                return BadRequest();
            }

            Contact contact = new Contact();

            contact.User = id;
            contact.Contact1 = idContact;

            _context.Contact.Add(contact);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return NotFound();
            }

            return CreatedAtRoute("getContactsByUserIdOld", new { id = id }, null);
        }

        // GET: /Users/5/tags
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/tags", Name = "getTagsByUserIdOld")]
        public async Task<ActionResult<IEnumerable<Object>>> getTagsByUserIDOld(int id) {

            var query =
                from tag in _context.Tag
                where tag.User == id
                select new {
                    tagId = tag.TagId,
                    tag = tag.Tag1,
                    user = tag.User,
                    tmsCreated = tag.TmsCreated
                };

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        // POST: /Users/5/tags
        //The user assosiated contacts of the user a returned
        [HttpPost("{id:int}/tags/add/{text}", Name = "addTagToUserOld")]
        public async Task<IActionResult> addTagToUserOld(int id, string text) {


            Tag tag = new Tag();

            tag.User = id;
            tag.Tag1 = text;

            _context.Tag.Add(tag);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException e) {
                return Conflict(e);
            }

            var result = new {
                tagId = tag.TagId,
                tag = tag.Tag1,
                user = tag.User,
                tmsCreated = tag.TmsCreated
            };

            return CreatedAtRoute("getTagByTagIdOld", new { id = tag.TagId }, result);
        }

        [HttpPost("{id:int}/edit", Name = "editUserOld")]
        public async Task<IActionResult> editUserOld(User editedUser) {

            var user = _context.User.FirstOrDefault(u => u.UserId == editedUser.UserId);

            Console.WriteLine(editedUser);
            Console.WriteLine("CourseAbr: " + editedUser.CourseAbr);
            Console.WriteLine("Course: " + editedUser.Course);
            Console.WriteLine("Specialization: " + editedUser.Specialization);
            Console.WriteLine("City: " + editedUser.City);
            Console.WriteLine("Bio: " + editedUser.Biography);
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
                return Conflict("Some data provided can't be changed");
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
                user.City = editedUser.City;
            }
            if (editedUser.Biography != null) {
                user.Biography = editedUser.Biography;
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

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id:int}", Name = "deleteUserByUserIdOld")]
        public async Task<ActionResult> deleteUserByUserIdOld(int id) {

            var user = await _context.User.FindAsync(id);

            if (user is null) {
                return NotFound();
            }

            _context.User.Remove(user);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }



}
