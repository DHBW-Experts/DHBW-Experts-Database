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

        [HttpGet("{tagId:int}/validations", Name = "getValidationsByTagId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> getValidationsByTagId(int tagId) {

            var query =
                from val in _context.TagValidations
                where val.Tag == tagId
                select new {
                    validationId = val.ValidationId,
                    tag = val.Tag,
                    validatedBy = val.ValidatedBy,
                    comment = val.Comment,
                    createdAt = val.CreatedAt
                };

            var result = await query.ToListAsync();

            if (result is null) {
                return NotFound();
            }
            return result;
        }
        
        public class ValidationIn {
            public int tag { get; set; }
            public string validatedBy { get; set; }
            public string comment { get; set; }
        }
        
        [HttpPost("{tagId:int}/validations", Name = "addTagValidation")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> addTagValidation(int tagId, ValidationIn valIn) {

            TagValidations validation = new TagValidations();

            if (tagId != valIn.tag) {
                return BadRequest();
            }
            
            validation.Tag = valIn.tag;
            validation.ValidatedBy = valIn.validatedBy;
            validation.Comment = valIn.comment;

            _context.TagValidations.Add(validation);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return Conflict();
            }

            return CreatedAtRoute("getValidationByValId", new { valId = validation.ValidationId }, new { 
                validationId = validation.ValidationId,
                tag = validation.Tag,
                validatedBy = validation.ValidatedBy,
                comment = validation.Comment,
                createdAt = validation.CreatedAt 
            });
        }

        [HttpGet("{tagId:int}", Name = "getTagByTagId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Object>>> getTagByTagId(int tagId) {

            var query =
                from tags in _context.Tags
                where tags.TagId == tagId
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
