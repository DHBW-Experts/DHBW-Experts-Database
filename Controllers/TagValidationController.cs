using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseAPI.Controllers {
    [Route("tag-validations")]
    [ApiController]
    public class TagValidationController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public TagValidationController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        [HttpGet("{valId:int}", Name = "getValidationByValId")]
        [Authorize]
        public async Task<ActionResult<Object>> getValidationsByTagId(int valId) {

            var query =
                from val in _context.TagValidations
                where val.ValidationId == valId
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

        [HttpDelete("{valId:int}", Name = "deleteTagValidationByTagId")]
        [Authorize]
        public async Task<ActionResult> deleteTagValByValId(int valId) {

            var val = await _context.TagValidations.FindAsync(valId);

            if (val is null) {
                return NotFound();
            }

            _context.TagValidations.Remove(val);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return Ok();
        }

    }

}
