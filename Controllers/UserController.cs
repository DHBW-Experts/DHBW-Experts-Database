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

        // GET: /Users/626db5fc4105f20069997435
        //The user assosiated with the specified ID is returned, if found.
        [HttpGet("{id}", Name = "getUserById")]
        public async Task<ActionResult<Object>> getUserById(string id) {
            var query =
                from user in _context.VwUsers
                where user.UserId == id
                select user;

            var result = await query.FirstOrDefaultAsync();
            if (result is null) {
                return NotFound();
            }
            return result;
        }

        //GET: /Users/rfid/432a9e7b626c87f
        [HttpGet("rfid/{rfidId}")]
        public async Task<ActionResult<Object>> getUserByRfidId(string rfidId) {
            var query =
                from user in _context.VwUsers
                join data in _context.UserData on user.UserId equals data.User
                where data.RfidId == rfidId
                select user;

            var result = await query.FirstOrDefaultAsync();
            if (result is null) {
                return NotFound();
            }
            return result;
        }

        // GET: /Users/5/contacts
        //The user assosiated contacts of the user a returned
        [HttpGet("{id}/contacts", Name = "getContactsByUserId")]
        public async Task<ActionResult<IEnumerable<Object>>> getContactsByUserID(string id) {

            var query =
                from contact in _context.Contacts
                join user in _context.VwUsers on contact.Contact equals user.UserId
                where contact.User == id
                select user;

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id}/contacts/{contactId}", Name = "deleteContactByUserId")]
        public async Task<ActionResult> deleteContactByUserId(string id, string contactId) {

            var query =
                from contact in _context.Contacts
                where contact.User == id && contact.Contact == contactId
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
        [HttpPost("{id}/contacts/{idContact}", Name = "addContactToUser")]
        public async Task<ActionResult> addContactToUser(string id, string idContact) {

            if (id == idContact) {
                return BadRequest();
            }

            Contacts contact = new Contacts();

            contact.User = id;
            contact.Contact = idContact;

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
        [HttpGet("{id}/tags", Name = "getTagsByUserId")]
        public async Task<ActionResult<IEnumerable<Object>>> getTagsByUserID(string id) {

            var query =
                from tag in _context.Tags
                where tag.User == id
                select new {
                    tagId = tag.TagId,
                    tag = tag.Tag,
                    user = tag.User,
                    createdAt = tag.CreatedAt
                };

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        // POST: /Users/5/tags
        //The user assosiated contacts of the user a returned
        [HttpPost("{id}/tags/{text}", Name = "addTagToUser")]
        public async Task<IActionResult> addTagToUser(string id, string text) {


            Tags tag = new Tags();

            tag.User = id;
            tag.Tag = text;

            _context.Tags.Add(tag);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException e) {
                return Conflict(e);
            }

            var result = new {
                tagId = tag.TagId,
                tag = tag.Tag,
                user = tag.User,
                createdAt = tag.CreatedAt
            };

            return CreatedAtRoute("getTagByTagId", new { id = tag.TagId }, result);
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id}/tags/{tagId:int}", Name = "deleteTagByTagId")]
        public async Task<ActionResult> deleteTagByTagId(int tagId) {

            var tag = await _context.Tags.FindAsync(tagId);

            _context.Tags.Remove(tag);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        public class UserWithRfid : VwUsers {
            public string RfidId { get; set; }
        }

        [HttpPatch("{id}", Name = "editUser")]
        public async Task<IActionResult> editUser(UserWithRfid editedUser) {

            var user = _context.UserData.FirstOrDefault(u => u.User == editedUser.UserId);

            if (editedUser.Course != null) {
                user.Course = editedUser.Course;
            }
            if (editedUser.CourseAbbr != null) {
                user.CourseAbbr = editedUser.CourseAbbr;
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

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id}", Name = "deleteUserByUserId")]
        public async Task<ActionResult> deleteUserByUserId(string id) {

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
