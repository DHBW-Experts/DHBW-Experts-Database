using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Model;

namespace DatabaseAPI.Controllers {
    [Route("Tags")]
    [ApiController]
    public class TagController : ControllerBase {
        private readonly DHBWExpertsdatabaseContext _context;

        //The context is managed by the WEBAPI and used here via Dependency Injection.
        public TagController(DHBWExpertsdatabaseContext context) {
            _context = context;
        }

        // GET: /Users/contacts/5
        //The user assosiated contacts of the user a returned
        [HttpGet("validations/{id:int}", Name = "GetValidationsByTagId")]
        public async Task<ActionResult<IEnumerable<TagValidation>>> GetValidationsByTagId(int id) {

            var query =
                from val in _context.TagValidations
                where val.Tag == id
                select val;

            var result = await query.ToListAsync();

            if (result is not null) {
                return result;
            }
            return NotFound();
        }

    }



}
