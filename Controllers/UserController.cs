using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseAPI.Controllers {
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public UserController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        [HttpGet("{userId}", Name = "getUserById")]
        [Authorize]
        public async Task<ActionResult<Object>> getUserById(string userId) {
            var query =
                from user in _context.VwUsers
                where user.UserId == userId
                select user;

            var result = await query.FirstOrDefaultAsync();
            if (result is null) {
                return NotFound();
            }
            return result;
        }

        [HttpGet("{userId}/contacts", Name = "getContactsByUserId")]
        [Authorize("read:profile")]
        public async Task<ActionResult<IEnumerable<Object>>> getContactsByUserID(string userId) {

            var query =
                from contact in _context.Contacts
                join user in _context.VwUsers on contact.Contact equals user.UserId
                where contact.User == userId
                select user;

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

        [HttpDelete("{userId}/contacts/{contactId}", Name = "deleteContactByUserId")]
        [Authorize("write:profile")]
        public async Task<ActionResult> deleteContactByUserId(string userId, string contactId) {

            var query =
                from contact in _context.Contacts
                where contact.User == userId && contact.Contact == contactId
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

        [HttpPost("{userId}/contacts/{idContact}", Name = "addContactToUser")]
        [Authorize("write:profile")]
        public async Task<ActionResult> addContactToUser(string userId, string idContact) {

            if (userId == idContact) {
                return BadRequest();
            }

            Contacts contact = new Contacts();

            contact.User = userId;
            contact.Contact = idContact;

            _context.Contacts.Add(contact);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return NotFound();
            }

            return CreatedAtRoute("getContactsByUserId", new { userId = userId }, null);
        }

        [HttpGet("{userId}/tags", Name = "getTagsByUserId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> getTagsByUserID(string userId) {

            var query =
                from tag in _context.Tags
                where tag.User == userId
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

        [HttpPost("{userId}/tags/{text}", Name = "addTagToUser")]
        [Authorize("write:profile")]
        public async Task<IActionResult> addTagToUser(string userId, string text) {


            Tags tag = new Tags();

            tag.User = userId;
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
                    validations = new int[0],
                    createdAt = tag.CreatedAt
                };

            return CreatedAtRoute("getTagByTagId", new { userId = tag.TagId }, result);
        }

        [HttpDelete("{userId}/tags/{tagId:int}", Name = "deleteTagByTagId")]
        [Authorize("write:profile")]
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

        [HttpPatch("{userId}", Name = "editUser")]
        [Authorize("write:profile")]
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

        [HttpDelete("{userId}", Name = "deleteUserByUserId")]
        [Authorize("write:profile")]
        public async Task<ActionResult> deleteUserByUserId(string userId) {

            var user = await _context.Users.FindAsync(userId);

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
