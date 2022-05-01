using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("auth0-tags")]
    [ApiController]
    public class Auth0TagController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public Auth0TagController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/validations", Name = "getValidationsByTagId")]
        public async Task<ActionResult<IEnumerable<Object>>> getValidationsByTagId(int id) {

            var query =
                from val in _context.TagValidation
                where val.Tag == id
                select new {
                    validationId = val.ValidationId,
                    tag = val.Tag,
                    validatedBy = val.ValidatedBy,
                    comment = val.Comment,
                    tmsCreated = val.TmsCreated
                };

            var result = await query.ToListAsync();

            if (result is null) {
                return NotFound();
            }
            return result;
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}", Name = "getTagByTagId")]
        public async Task<ActionResult<IEnumerable<Object>>> getTagByTagId(int id) {

            var query =
                from tags in _context.Tag
                where tags.TagId == id
                select new {
                    tagId = tags.TagId,
                    tag = tags.Tag1,
                    user = tags.User,
                    tmsCreated = tags.TmsCreated
                };

            var result = await query.ToListAsync();

            if (result is null) {
                return NotFound();
            }
            return result;
        }


        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id:int}", Name = "deleteTagByTagId")]
        public async Task<ActionResult> deleteTagByTagId(int id) {

            var tag = await _context.Tag.FindAsync(id);

            if (tag is null) {
                return NotFound();
            }

            _context.Tag.Remove(tag);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }



}
