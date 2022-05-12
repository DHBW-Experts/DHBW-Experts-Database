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
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public TagController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}/validations", Name = "getValidationsByTagId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> getValidationsByTagId(int id) {

            var query =
                from val in _context.TagValidations
                where val.Tag == id
                select new {
                    validationId = val.ValidationId,
                    tag = val.Tag,
                    validatedBy = val.ValidatedBy,
                    comment = val.Comment,
                    tmsCreated = val.CreatedAt
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> getTagByTagId(int id) {

            var query =
                from tags in _context.Tags
                where tags.TagId == id
                select new {
                    tagId = tags.TagId,
                    tag = tags.Tag,
                    user = tags.User,
                    createdAt = tags.CreatedAt
                };

            var result = await query.ToListAsync();

            if (result is null) {
                return NotFound();
            }
            return result;
        }

    }

}
