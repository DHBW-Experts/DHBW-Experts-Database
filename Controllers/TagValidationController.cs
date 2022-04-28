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
                from val in _context.TagValidation
                where val.ValidationId == id
                select new {
                    validationId = val.ValidationId,
                    tag = val.Tag,
                    validatedBy = val.ValidatedBy,
                    comment = val.Comment,
                    tmsCreated = val.TmsCreated
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

            var val = await _context.TagValidation.FindAsync(id);

            if (val is null) {
                return NotFound();
            }

            _context.TagValidation.Remove(val);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }



}
