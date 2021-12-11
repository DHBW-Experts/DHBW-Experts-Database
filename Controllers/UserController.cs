using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public UserController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/5
        //The user assosiated with the specified ID is returned, if found.
        [HttpGet("{id:int}", Name = "getUserById")]
        public async Task<ActionResult<Object>> getUserById(int id) {
            var query =  
                from user in _context.Users
                join loc in _context.Dhbws on user.Dhbw equals loc.Location
                where user.UserId == id
                select new {
                    userId = id,
                    firstName = user.Firstname,
                    lastname = user.Lastname,
                    dhbw = user.Dhbw,
                    course = user.Course,
                    courseAbr = user.CourseAbr,
                    specialization = user.Specialization,
                    email = user.EmailPrefix + "@" + loc.EmailDomain,
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

        //GET: /Users/rfid/432a9e7b626c87f
        [HttpGet("rfid/{rfidId}")]
        public async Task<ActionResult<Object>> getUser(string rfidId) {
            var query =  
                from user in _context.Users
                join loc in _context.Dhbws on user.Dhbw equals loc.Location
                where user.RfidId == rfidId
                select new {
                    userId = user.UserId,
                    firstName = user.Firstname,
                    lastname = user.Lastname,
                    dhbw = user.Dhbw,
                    course = user.Course,
                    courseAbr = user.CourseAbr,
                    specialization = user.Specialization,
                    email = user.EmailPrefix + "@" + loc.EmailDomain,
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

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/contacts", Name = "getContactsByUserId")]
        public async Task<ActionResult<IEnumerable<Object>>> getContacsByUserID(int id) {

            var query =
                from contact in _context.Contacts
                join user in _context.Users on contact.Contact1 equals user.UserId
                join loc in _context.Dhbws on user.Dhbw equals loc.Location
                where contact.User == id
                select new {
                    userId = user.UserId,
                    firstName = user.Firstname,
                    lastname = user.Lastname,
                    dhbw = user.Dhbw,
                    course = user.Course,
                    courseAbr = user.CourseAbr,
                    specialization = user.Specialization,
                    email = user.EmailPrefix + "@" + loc.EmailDomain,
                    city = user.City,
                    biographie = user.Bio,
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
        [HttpDelete("{id:int}/contacts/{contactId}", Name = "deleteContactByUserId")]
        public async Task<ActionResult> deleteContactByUserId(int id, int contactId) {

            var query =
                from contact in _context.Contacts
                where contact.User == id && contact.Contact1 == contactId
                select contact;

            var c = await query.FirstOrDefaultAsync();

            if (c is null) {
                return NotFound();
            }

            _context.Contacts.Remove(c);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpPost("{id:int}/contacts/add/{idContact:int}", Name = "addContactToUser")]
        public async Task<ActionResult> addContactToUser(int id, int idContact) {

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
                return NotFound();
            }

            return CreatedAtRoute("getContactsByUserId", new { id = id }, null);
        }

        // GET: /Users/5/tags
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/tags", Name = "getTagsByUserId")]
        public async Task<ActionResult<IEnumerable<Object>>> getTagsByUserID(int id) {

            var query =
                from tag in _context.Tags
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
        [HttpPost("{id:int}/tags/add/{text}", Name = "addTagToUser")]
        public async Task<IActionResult> addTagToUser(int id, string text) {
            

            Tag tag = new Tag();

            tag.User = id;
            tag.Tag1 = text;

            _context.Tags.Add(tag);

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

            return CreatedAtRoute("getTagByTagId", new { id = tag.TagId }, result);
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

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id:int}", Name = "deleteUserByUserId")]
        public async Task<ActionResult> deleteUserByUserId(int id) {

            var user = await _context.Users.FindAsync(id);

            if (user is null) {
                return NotFound();
            }

            _context.Users.Remove(user);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }



}
