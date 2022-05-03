using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("tag-validations")]
    [ApiController]
    public class TagValidationController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public TagValidationController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpGet("{id:int}", Name = "getValidationByValId")]
        public async Task<ActionResult<Object>> getValidationsByTagId(int id) {

            var query =
                from val in _context.Auth0TagValidations
                where val.ValidationId == id
                select new {
                    validationId = val.ValidationId,
                    tag = val.Tag,
                    validatedBy = val.ValidatedBy,
                    comment = val.Comment,
                    CreatedAt = val.CreatedAt
                };

            var result = await query.FirstOrDefaultAsync();

            if (result is null) {
                return NotFound();
            }
            return result;
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpDelete("{id:int}", Name = "deleteTagValidationByTagId")]
        public async Task<ActionResult> deleteTagValByValId(int id) {

            var val = await _context.Auth0TagValidations.FindAsync(id);

            if (val is null) {
                return NotFound();
            }

            _context.Auth0TagValidations.Remove(val);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }



}
